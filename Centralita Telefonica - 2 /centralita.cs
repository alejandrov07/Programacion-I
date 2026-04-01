import java.util.ArrayList;
import java.util.List;

abstract class Llamada {
    private String numeroOrigen;
    private String numeroDestino;
    private int duracion;

    public Llamada(String numeroOrigen, String numeroDestino, int duracion) {
        this.numeroOrigen = numeroOrigen;
        this.numeroDestino = numeroDestino;
        this.duracion = duracion;
    }

    public String getNumeroOrigen() {
        return numeroOrigen;
    }

    public String getNumeroDestino() {
        return numeroDestino;
    }

    public int getDuracion() {
        return duracion;
    }

    public abstract double calcularCosto();

    @Override
    public String toString() {
        return "Origen: " + numeroOrigen +
               ", Destino: " + numeroDestino +
               ", Duracion: " + duracion +
               "s, Costo: $" + String.format("%.2f", calcularCosto());
    }
}

class LlamadaLocal extends Llamada {
    public LlamadaLocal(String numeroOrigen, String numeroDestino, int duracion) {
        super(numeroOrigen, numeroDestino, duracion);
    }

    @Override
    public double calcularCosto() {
        return getDuracion() * 0.15;
    }
}

class LlamadaProvincial extends Llamada {
    private int franja;

    public LlamadaProvincial(String numeroOrigen, String numeroDestino, int duracion, int franja) {
        super(numeroOrigen, numeroDestino, duracion);
        this.franja = franja;
    }

    public int getFranja() {
        return franja;
    }

    @Override
    public double calcularCosto() {
        double costoPorSegundo;
        switch (franja) {
            case 1:
                costoPorSegundo = 0.20;
                break;
            case 2:
                costoPorSegundo = 0.25;
                break;
            case 3:
                costoPorSegundo = 0.30;
                break;
            default:
                costoPorSegundo = 0;
        }
        return getDuracion() * costoPorSegundo;
    }

    @Override
    public String toString() {
        return super.toString() + ", Franja: " + franja;
    }
}

class Centralita {
    private List<Llamada> llamadas;
    private int totalLlamadas;
    private double facturacionTotal;

    public Centralita() {
        llamadas = new ArrayList<>();
        totalLlamadas = 0;
        facturacionTotal = 0;
    }

    public void registrarLlamada(Llamada llamada) {
        llamadas.add(llamada);
        totalLlamadas++;
        facturacionTotal += llamada.calcularCosto();
        System.out.println(llamada);
    }

    public void mostrarInforme() {
        System.out.println("\nINFORME FINAL");
        System.out.println("Total de llamadas: " + totalLlamadas);
        System.out.println("Facturacion total: $" + String.format("%.2f", facturacionTotal));
    }
}

public class Practica2 {
    public static void main(String[] args) {
        Centralita centralita = new Centralita();

        centralita.registrarLlamada(new LlamadaLocal("8091111111", "8092222222", 60));
        centralita.registrarLlamada(new LlamadaProvincial("8093333333", "8094444444", 120, 1));
        centralita.registrarLlamada(new LlamadaProvincial("8095555555", "8096666666", 90, 2));
        centralita.registrarLlamada(new LlamadaLocal("8097777777", "8098888888", 30));
        centralita.registrarLlamada(new LlamadaProvincial("8099999999", "8090000000", 45, 3));

        centralita.mostrarInforme();
    }
}
