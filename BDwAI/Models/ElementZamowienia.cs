namespace BDwAI.Models
{
    public class ElementZamowienia
    {
        public int Id { get; set; }

        // Do jakiego zamówienia należy ten element
        public int ZamowienieId { get; set; }
        public Zamowienie Zamowienie { get; set; }

        // Jaki to produkt
        public int ProduktId { get; set; }
        public Produkt Produkt { get; set; }

        public int Ilosc { get; set; }
        public decimal CenaJednostkowa { get; set; } // Cena w momencie zakupu
    }
}