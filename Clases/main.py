from flask import Flask, request, jsonify
from functools import wraps
import jwt
import os
import random
from dotenv import load_dotenv
load_dotenv()
app = Flask(__name__)

# TODO: en producción, define JWT_SECRET_KEY como variable de entorno real.
# El fallback "secret" solo existe para que puedas correr esto en local sin configurar nada.

secret_key = os.environ.get('JWT_SECRET_KEY', 'secret')


@app.route('/login', methods=['POST'])
def login():
    try:
        username = request.json.get('username')
        password = request.json.get('password')
        if username is None or password is None:
            return jsonify({'message': 'Both username and password are required'}), 400
        if username == "admin" and password == "123":
            token = jwt.encode({'username': username}, secret_key, algorithm='HS256')
            return jsonify({'token': token}), 200

        return jsonify({'message': 'Authentication failed'}), 401
    except Exception as e:
        print(e)
        return jsonify({'message': 'Internal server error'}), 500


def verify_token(func):
    @wraps(func)  # <- FIX: sin esto, todas las rutas decoradas se llaman "wrapper"
    def wrapper(*args, **kwargs):  # y Flask revienta al registrar la segunda ruta protegida.
        token = request.headers.get('Authorization', '')
        if not token:
            return jsonify({'message': 'Token not provided'}), 401
        token_parts = token.split(" ")
        if len(token_parts) != 2 or token_parts[0].lower() != 'bearer':
            return jsonify({'message': 'Invalid token format'}), 401
        token = token_parts[1]
        try:
            payload = jwt.decode(token, secret_key, algorithms=['HS256'])
            request.username = payload['username']
            return func(*args, **kwargs)
        except jwt.ExpiredSignatureError:
            return jsonify({'message': 'Token expired'}), 403
        except jwt.InvalidTokenError:
            return jsonify({'message': 'Invalid token'}), 403
    return wrapper


@app.route('/protected', methods=['GET'])
@verify_token
def protected():
    return jsonify({'message': 'You have access'}), 200


def _get_numbers():
    """Extrae y valida 'a' y 'b' del body JSON. Lanza ValueError si faltan o no son numéricos."""
    data = request.get_json(silent=True) or {}
    a = data.get('a')
    b = data.get('b')
    if a is None or b is None:
        raise ValueError("Both 'a' and 'b' are required")
    try:
        a = float(a)
        b = float(b)
    except (TypeError, ValueError):
        raise ValueError("'a' and 'b' must be numbers")
    return a, b


@app.route('/multiply', methods=['POST'])
@verify_token
def multiply():
    try:
        a, b = _get_numbers()
        return jsonify({'result': a * b}), 200
    except ValueError as e:
        return jsonify({'message': str(e)}), 400
    except Exception as e:
        print(e)
        return jsonify({'message': 'Internal server error'}), 500


@app.route('/divide', methods=['POST'])
@verify_token
def divide():
    try:
        a, b = _get_numbers()
        if b == 0:
            return jsonify({'message': "Division by zero is not allowed"}), 400
        return jsonify({'result': a / b}), 200
    except ValueError as e:
        return jsonify({'message': str(e)}), 400
    except Exception as e:
        print(e)
        return jsonify({'message': 'Internal server error'}), 500


@app.route('/game/rps', methods=['POST'])
def rock_paper_scissors():
    """
    Piedra, papel o tijera. Un solo request = un solo round (sin estado en el servidor).
    Body esperado: {"choice": "piedra" | "papel" | "tijera"}
    """
    options = ['piedra', 'papel', 'tijera']
    data = request.get_json(silent=True) or {}
    player_choice = str(data.get('choice', '')).lower().strip()

    if player_choice not in options:
        return jsonify({'message': f'Choice must be one of {options}'}), 400

    cpu_choice = random.choice(options)

    if player_choice == cpu_choice:
        result = 'draw'
    elif (
        (player_choice == 'piedra' and cpu_choice == 'tijera') or
        (player_choice == 'papel' and cpu_choice == 'piedra') or
        (player_choice == 'tijera' and cpu_choice == 'papel')
    ):
        result = 'win'
    else:
        result = 'lose'

    return jsonify({
        'your_choice': player_choice,
        'cpu_choice': cpu_choice,
        'result': result
    }), 200


if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5000, debug=True)
