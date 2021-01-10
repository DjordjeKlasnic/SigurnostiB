using Manager;
using ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        public string group = "";
        static void Main(string[] args)
        {

            /// Define the expected service certificate. It is required to establish cmmunication using certificates.
            string srvCertCN = "WCFService";

            NetTcpBinding binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            /// Use CertManager class to obtain the certificate based on the "srvCertCN" representing the expected service identity.
            X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, srvCertCN);
            EndpointAddress address = new EndpointAddress(new Uri("net.tcp://localhost:9999/Receiver"),
                                      new X509CertificateEndpointIdentity(srvCert));
            using (WCFClient proxy = new WCFClient(binding, address))
            {
                bool rez = proxy.TestCommunication();

                string[] groups = proxy.Credentials.ClientCertificate.Certificate.Subject.Split('=', ' ');
                string group = groups[3];

               
                string selection ="";
                do
                {
                    if (group == "Client")
                    {
                        Console.Clear();
                        Console.WriteLine("===================================================");
                        Console.WriteLine("                    Welcome ");
                        Console.WriteLine();
                        Console.WriteLine("1. Napravi rezervaciju.");
                        Console.WriteLine("2. Plati rezervaciju.");
                        Console.WriteLine("3. Izadji iz menija.");
                        Console.WriteLine();
                        Console.WriteLine("===================================================");
                        selection = Console.ReadLine();
                        switch (selection)
                        {
                            case "1":
                                Console.Clear();
                                Console.WriteLine("===================================================");
                                Console.WriteLine();
                                List<Concert> list = proxy.AllConcerts();
                                foreach (Concert concert in list)
                                {
                                    Console.WriteLine(concert);
                                }

                                Console.WriteLine();
                                Console.Write("Unesite ID koncerta za koji zelite da rezervisete karte:");
                                string id = Console.ReadLine();
                                Console.Write("Koliko karata zelite da rezervisete:");
                                string brKarata = Console.ReadLine();
                                proxy.MakeReservation(id, brKarata);
                                break;
                            case "2":
                                Console.Clear();
                                Console.WriteLine("===================================================");
                                Console.WriteLine();
                                int balance = proxy.AccountBalance();
                                Console.WriteLine("Vase stanje na racunu je: {0}",balance);
                                List<Reservation> list1 = proxy.AllNotPayedReservations();
                                foreach (Reservation r in list1)
                                {
                                    Console.WriteLine(r);
                                }
                                Console.WriteLine();
                                Console.Write("Unesite ID rezervacije koju zelite da platite: ");
                                string idReservation = Console.ReadLine();
                                proxy.PayReservation(idReservation);
                                break;
                            case "3":
                                return;
                            default:
                                break;
                        }

                      }
                    else if (group == "Admin")
                    {
                        Console.Clear();
                        Console.WriteLine("===================================================");
                        Console.WriteLine("                   Welcome Admin");
                        Console.WriteLine();
                        Console.WriteLine("1. Dodaj koncert.");
                        Console.WriteLine("2. Izmeni koncert.");
                        Console.WriteLine("3. Izadji iz menija.");
                        Console.WriteLine();
                        Console.WriteLine("===================================================");
                        selection = Console.ReadLine();
                        switch (selection)
                        {
                            case "1":
                                Console.Clear();
                                Console.WriteLine("===================================================");
                                Console.WriteLine();
                                string naziv = "";
                                do
                                {
                                    Console.Write("Unesite naziv koncerta:");
                                    naziv = Console.ReadLine();
                                } while (naziv.Length == 0);

                                string lokacija = "";
                                do
                                {
                                    Console.Write("Unesite lokaciju koncerta:");
                                    lokacija = Console.ReadLine();
                                } while (lokacija.Length == 0);

                                string godina = "";
                                do
                                {
                                    Console.WriteLine("Unesite godinu:");
                                    godina = Console.ReadLine();
                                } while (godina.Length == 0);

                                string mesec = "";
                                do
                                {
                                    Console.WriteLine("Unesite mesec:");
                                    mesec = Console.ReadLine();
                                } while (mesec.Length == 0);

                                string dan = "";
                                do
                                {
                                    Console.WriteLine("Unesite dan:");
                                    dan = Console.ReadLine();
                                } while (dan.Length == 0);

                                string cena = "";
                                do
                                {
                                    Console.WriteLine("Unesite cenu karte:");
                                    cena = Console.ReadLine();
                                } while (cena.Length == 0);
                                DateTime d = new DateTime(Int32.Parse(godina), Int32.Parse(mesec), Int32.Parse(dan));
                                proxy.AddConcert(naziv, d, lokacija, Int32.Parse(cena));
                                break;
                            case "2":
                                Console.Clear();
                                Console.WriteLine("===================================================");
                                Console.WriteLine();
                                List<Concert> list = proxy.AllConcerts();
                                foreach (Concert concert in list)
                                {
                                    Console.WriteLine(concert);
                                }

                                Console.WriteLine();
                                Console.WriteLine("Unesite ID konceta koji zelite da izmenite:");
                                string idNewConcert = Console.ReadLine();
                                Console.WriteLine("Unesite novu lokaciju:");
                                string loc = Console.ReadLine();
                                Console.WriteLine("Unesite novu cenu karti:");
                                string price = Console.ReadLine();
                                proxy.EditConcert(idNewConcert, loc, price);

                                break;
                            case "3":
                                return;
                             default:
                                break;
                                
                        }
                    }
                    
                    if (selection == ""){
                        Console.WriteLine("Niste izabrali ni jednu od ponudjenih opcija");
                    }
                }
                while (selection != "3");
                
                Console.ReadLine();
            }
        }
    }
}
