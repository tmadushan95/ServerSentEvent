namespace ServerSentEvent.Api.Gen
{
    public sealed record Order
    {
        public string Name { get; init; } = string.Empty;
        public string Price { get; init; } = string.Empty;
    }

    public static class OrderGen
    {
        private static readonly string[] foods = [

            "Pizza",
            "Burger",
            "Pasta",
            "Salad",
            "Sushi"
        ];

        public static Order CreateOrder()
        {
            return new Order
            {
                Name = foods[Random.Shared.Next(0, 5)],
                Price = Random.Shared.Next(5, 25).ToString()
            };
        }
    }
}
