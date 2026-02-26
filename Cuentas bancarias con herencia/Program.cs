using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

namespace Cuentas_bancarias_con_herencia
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Crear y depositar en cuentas (ingrese los datos solicitados)");

            Console.WriteLine();
            Console.WriteLine("-- Crear Cuenta de Ahorros --");
            int numA;
            Console.WriteLine("Número de cuenta: ");
            while (!int.TryParse(Console.ReadLine(), out numA))
            {
                Console.WriteLine("Entrada inválida. Ingrese un número entero para la cuenta: ");
            }
            Console.WriteLine("Titular: ");
            string titA = Console.ReadLine();
            Console.WriteLine("Tasa de interés anual del 5%");
            decimal tasa = 0.05m;
            CuentaAhorros ahorros = new CuentaAhorros(numA, titA, tasa);

            Console.WriteLine($"Saldo inicial de la cuenta de ahorros: ${ahorros.ObtenerSaldo():F2}");
            Console.Write("¿Desea retirar de la cuenta de ahorros? (s/n): ");
            string respRet = Console.ReadLine();
            if (!string.IsNullOrEmpty(respRet) && respRet.ToLower().StartsWith("s"))
            {
                while (true)
                {
                    Console.Write("Ingrese el monto a retirar: ");
                    decimal montoRet;
                    while (!decimal.TryParse(Console.ReadLine(), out montoRet) || montoRet <= 0m)
                    {
                        Console.Write("Entrada inválida. Ingrese una cantidad positiva: ");
                    }

                    decimal nuevoSaldo = ahorros.ObtenerSaldo() - montoRet;
                    if (nuevoSaldo < 500m)
                    {
                        Console.WriteLine("Retiro denegado. El saldo no puede quedar por debajo de $500.");
                        Console.Write("¿Desea intentar otro monto? (s/n): ");
                        string intentar = Console.ReadLine();
                        if (string.IsNullOrEmpty(intentar) || !intentar.ToLower().StartsWith("s"))
                        {
                            break; 
                        }
                        else
                        {
                            continue; 
                        }
                    }

                    ahorros.Retirar(montoRet);
                    break;
                }
            }

            Console.WriteLine();
            Console.WriteLine("-- Crear Cuenta Corriente --");
            int numC;
            Console.WriteLine("Número de cuenta: ");
            while (!int.TryParse(Console.ReadLine(), out numC))
            {
                Console.WriteLine("Entrada inválida. Ingrese un número entero para la cuenta: ");
            }
            Console.WriteLine("Titular: ");
            string titC = Console.ReadLine();
            Console.WriteLine("Comisión mensual 10");
            decimal comision = 10;
          
            Console.WriteLine("Límite de sobregiro 500");
            decimal limite = 500;
          
            CuentaCorriente corriente = new CuentaCorriente(numC, titC, comision, limite);
            Console.WriteLine("Cuánto desea depositar en la cuenta corriente?: ");
            decimal depC;
            while (!decimal.TryParse(Console.ReadLine(), out depC) || depC <= 0m)
            {
                Console.WriteLine("Entrada inválida. Ingrese una cantidad positiva: ");
            }
            corriente.Depositar(depC);

            Console.WriteLine();
            Console.WriteLine("-- Crear Cuenta Nómina --");
            int numN;
            Console.WriteLine("Número de cuenta: ");
            while (!int.TryParse(Console.ReadLine(), out numN))
            {
                Console.WriteLine("Entrada inválida. Ingrese un número entero para la cuenta: ");
            }
            Console.WriteLine("Titular: ");
            string titN = Console.ReadLine();
            Console.WriteLine("Empresa: ");
            string empresa = Console.ReadLine();
            CuentaNomina nomina = new CuentaNomina(numN, titN, empresa);
            Console.WriteLine("Cuánto desea depositar en la cuenta nómina?: ");
            decimal depN;
            while (!decimal.TryParse(Console.ReadLine(), out depN) || depN <= 0m)
            {
                Console.WriteLine("Entrada inválida. Ingrese una cantidad positiva: ");
            }
            nomina.Depositar(depN);

            Console.WriteLine();
            Console.WriteLine("--- Información final de las cuentas ---");
            Console.WriteLine();
            Console.WriteLine("Cuenta Ahorros:");
            ahorros.MostrarInformacion();
            Console.WriteLine($"Intereses (saldo * tasa): ${ahorros.CalcularIntereses():F2}");
            Console.WriteLine();
            Console.WriteLine("Cuenta Corriente:");
            corriente.MostrarInformacion();
            Console.WriteLine();
            Console.WriteLine("Cuenta Nómina:");
            nomina.MostrarInformacion();

            Console.WriteLine();
            Console.WriteLine("Presione cualquier tecla para salir...");
            Console.ReadKey();
        }
    }
    public class CuentaBancaria
    {

        protected int numeroCuenta;
        protected string titular;
        protected decimal saldo = 0m;
        public CuentaBancaria(int numeroCuenta, string titular)
        {
            this.numeroCuenta = numeroCuenta;
            this.titular = titular;
            this.saldo = 0m;
        }

        public void PedirInformacion()
        {
            Console.WriteLine("Ingrese el numero de cuenta: ");
            int numero;
            while (!int.TryParse(Console.ReadLine(), out numero))
            {
                Console.WriteLine("Entrada inválida. Ingrese un número de cuenta válido: ");
            }
            this.numeroCuenta = numero;

            Console.WriteLine("Ingrese el nombre del titular: ");
            this.titular = Console.ReadLine();

            Console.WriteLine($"Su Saldo Actual es de: ${saldo:F2}");
        }
        public void MostrarInformacion()
        {
            Console.WriteLine($"Numero de cuenta: {numeroCuenta}");
            Console.WriteLine($"Titular: {titular}");
            Console.WriteLine($"Saldo: ${saldo:F2}");
        }
        public void Depositar()
        {
            Console.WriteLine("Ingrese la cantidad a depositar: ");
            decimal cantidad;
            while (!decimal.TryParse(Console.ReadLine(), out cantidad) || cantidad <= 0m)
            {
                Console.WriteLine("Entrada inválida. Ingrese una cantidad positiva: ");
            }
            saldo += cantidad;
            Console.WriteLine($"Depósito exitoso. Saldo total: ${saldo:F2}");
        }

        public void Depositar(decimal cantidad)
        {
            if (cantidad <= 0m)
            {
                Console.WriteLine("La cantidad a depositar debe ser mayor que 0.");
                return;
            }
            saldo += cantidad;
            Console.WriteLine($"Depósito de ${cantidad:F2} realizado. Saldo: ${saldo:F2}");
        }

        public decimal ObtenerSaldo()
        {
            return saldo;
        }

        public virtual bool Retirar(decimal monto)
        {
            if (monto <= 0m)
            {
                Console.WriteLine("El monto a retirar debe ser mayor que 0.");
                return false;
            }
            if (monto > saldo)
            {
                Console.WriteLine("Fondos insuficientes.");
                return false;
            }
            saldo -= monto;
            Console.WriteLine($"Retiro exitoso. Saldo actual: ${saldo:F2}");
            return true;
        }

        public virtual decimal CalcularIntereses()
        {
            return 0m;
        }
    }
    public class CuentaAhorros : CuentaBancaria
    {
        private decimal tasaInteres; 
        private decimal comisionMensual = 5m; 

        public decimal ComisionMensual { get { return comisionMensual; } }

        public CuentaAhorros(int numeroCuenta, string titular, decimal tasaInteres) : base(numeroCuenta, titular)
        {
            this.tasaInteres = tasaInteres;
            this.saldo = 1000m;
        }

        public override decimal CalcularIntereses()
        {
            return saldo * tasaInteres;
        }

        public override bool Retirar(decimal monto)
        {
            if (monto <= 0m)
            {
                Console.WriteLine("El monto a retirar debe ser mayor que 0.");
                return false;
            }

            decimal nuevoSaldo = saldo - monto;
            if (nuevoSaldo < 500m)
            {
                Console.WriteLine("Retiro denegado. El saldo no puede quedar por debajo de $500.");
                return false;
            }

            saldo = nuevoSaldo;
            Console.WriteLine($"Retiro exitoso. Saldo actual: ${saldo:F2}");
            return true;
        }
    }
    public class CuentaCorriente : CuentaBancaria
    {
        private decimal comisionMensual;
        private decimal limiteSobregiro;

        public CuentaCorriente(int numeroCuenta, string titular, decimal comisionMensual, decimal limiteSobregiro) : base(numeroCuenta, titular)
        {
            this.comisionMensual = comisionMensual;
            this.limiteSobregiro = limiteSobregiro;
        }

        public override decimal CalcularIntereses()
        {
            return 0m;
        }

        public override bool Retirar(decimal monto)
        {
            if (monto <= 0m)
            {
                Console.WriteLine("El monto a retirar debe ser mayor que 0.");
                return false;
            }

            decimal disponible = saldo + limiteSobregiro;
            if (monto > disponible)
            {
                Console.WriteLine("Retiro denegado. Excede el límite de sobregiro.");
                return false;
            }

            saldo -= monto;

            if (saldo < 0m)
            {
                saldo -= 20m;
                Console.WriteLine("La cuenta quedó en negativo. Se aplicó una comisión de $20.");
            }

            Console.WriteLine($"Retiro realizado. Saldo actual: ${saldo:F2}");
            return true;
        }
    }

    public class CuentaNomina : CuentaBancaria
    {
        private string empresa;

        public CuentaNomina(int numeroCuenta, string titular, string empresa) : base(numeroCuenta, titular)
        {
            this.empresa = empresa;
        }

        public override decimal CalcularIntereses()
        {
            return saldo * 0.01m;
        }

        public override bool Retirar(decimal monto)
        {
            if (monto <= 0m)
            {
                Console.WriteLine("El monto a retirar debe ser mayor que 0.");
                return false;
            }
            if (monto > saldo)
            {
                Console.WriteLine("Fondos insuficientes en cuenta nómina.");
                return false;
            }
            saldo -= monto;
            Console.WriteLine($"Retiro exitoso. Saldo actual: ${saldo:F2}");
            return true;
        }
    }
}