using System.Security.Claims;


namespace jsolo.simpleinventory.impl.identity
{
    public static class Claims
    {
        public static Claim AppScope => new("ApplicationScope", "jsolo.simpleinventory.*");

        public static Claim Developer = new Claim("Apuare.customersAppDeveloper", "true");

        public static Claim Administrator = new Claim("Apuare.customersAppAdministrator", "true");
    }
}
