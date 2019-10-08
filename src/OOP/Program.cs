using System;

namespace OOP
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var weatherParser = new DataParser<WeatherItem>(
                lines => int.TryParse(lines[0], out var val),
                (lines, fixDigit) => new WeatherItem
                {
                    Day = int.Parse(lines[0]),
                    MinTemperature = decimal.Parse(fixDigit(lines[1])),
                    MaxTemperature = decimal.Parse(fixDigit(lines[2]))
                });

            var weatherProcessor = new DataProcessor<WeatherItem>((ac, item) =>
            {
                var diffAc = Math.Abs(ac.MaxTemperature - ac.MinTemperature);
                var diffItem = Math.Abs(item.MaxTemperature - item.MinTemperature);

                if (diffAc < diffItem)
                    return ac;

                return item;
            });

            var footballParser = new DataParser<FootballTeam>(
                lines => true,
                (lines, fixDigit) => new FootballTeam
                {
                    Name = lines[1],
                    Scored = int.Parse(fixDigit(lines[6])),
                    Missed = int.Parse(fixDigit(lines[8]))
                });

            var footballProcessor = new DataProcessor<FootballTeam>((ac, item) =>
            {
                var diffAc = Math.Abs(ac.Scored - ac.Missed);
                var diffItem = Math.Abs(item.Scored - item.Missed);

                if (diffAc < diffItem)
                    return ac;

                return item;
            });

            var fileReader = new FileReader(new char[] { ' ', '\t' }, true);

            var weatherPrinter = new Printer<WeatherItem>(
                @"Data\weather.dat",
                fileReader,
                weatherParser,
                weatherProcessor,
                item => Console.WriteLine($"{item.Day} - {item.MinTemperature} - {item.MaxTemperature}"),
                item => Console.WriteLine($"Day with min spread: {item.Day}"));
            weatherPrinter.Print();

            var footballPrinter = new Printer<FootballTeam>(
                @"Data\football.dat",
                fileReader,
                footballParser,
                footballProcessor,
                item => Console.WriteLine($"{item.Name} - {item.Scored} - {item.Missed}"),
                item => Console.WriteLine($"Team with min spread: {item.Name}"));
            footballPrinter.Print();

            Console.ReadKey();
        }
    }
}
