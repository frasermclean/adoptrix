namespace Adoptrix.Persistence;

public static class SeedData
{
    public static class Species
    {
        public const int Dog = 1;
        public const int Cat = 2;
        public const int Bird = 3;
    }

    public static class Breeds
    {
        // dogs
        public const int FrenchBulldog = 1;
        public const int LabradorRetriever = 2;
        public const int GoldenRetriever = 3;
        public const int GermanShepherd = 4;
        public const int Poodle = 5;
        public const int Bulldog = 6;
        public const int Rottweiler = 7;
        public const int Beagle = 8;
        public const int Dachshund = 9;
        public const int GermanShorthairedPointer = 10;

        // cats
        public const int Ragdoll = 11;
        public const int MaineCoon = 12;
        public const int Persian = 13;
        public const int DomesticShorthair = 14;
        public const int DevonRex = 15;

        // birds
        public const int Budgerigar = 16;
        public const int Cockatiel = 17;
        public const int AfricanGreyParrot = 18;
        public const int Lovebird = 19;
        public const int Canary = 20;
    }

    public static class Animals
    {
        public static readonly Guid Alberto = Guid.Parse("5edcfb8c-296a-434f-913f-967c58073ca2");
        public static readonly Guid Barry = Guid.Parse("c387d7dc-18f0-4ecb-bc8e-10c6c9ba9e1f");
        public static readonly Guid Ginger = Guid.Parse("a8a4897a-6594-4796-9b25-f54ca71bbbc7");
        public static readonly Guid Percy = Guid.Parse("7e92871b-7219-495f-945d-fc8b5ba78829");
    }
}
