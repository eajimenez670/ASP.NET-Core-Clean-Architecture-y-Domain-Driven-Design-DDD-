using CleanArchitecture.Data;
using CleanArchitecture.Domain;
using Microsoft.EntityFrameworkCore;

StreamerDbContext dbContext = new();

// await AddNewRecords();
// await QueryStreaming();
// await QueryFilter();
// await QueryMethods();
await QueryLinq();

Console.WriteLine("Presione cualquier tecla para terminar el programa");
Console.ReadKey();

async Task QueryLinq()
{
    Console.WriteLine("ingrese el servcio de streaming");
    var streamerName = Console.ReadLine();

    var streamers = await (from i in dbContext.Streamers
                           where EF.Functions.Like(i.Nombre, $"%{streamerName}%")
                           select i).ToListAsync();


    foreach (var streamer in streamers)
    {
        Console.WriteLine($"{streamer.Id} - {streamer.Nombre}");
    }


}

async Task QueryMethods()
{
    var streamer = dbContext.Streamers;
    var firstAsync = await streamer.Where(x => x.Nombre.Contains("a")).FirstAsync();
    var firstOrDefaultAsync = await streamer.Where(x => x.Nombre.Contains("a")).FirstOrDefaultAsync();
    var firstOrDefaultAsync_V2 = await streamer.FirstOrDefaultAsync(x => x.Nombre.Contains("a"));

    var singleAsync = await streamer.Where(x => x.Id == 1).SingleAsync();
    var singleOrDefaultAsync = await streamer.Where(x => x.Id == 1).SingleOrDefaultAsync();


    var resultado = await streamer.FindAsync(1);

    var count = await streamer.CountAsync();
    var longCount = await streamer.LongCountAsync();
    var min = await streamer.MinAsync();
    var max = await streamer.MaxAsync();
}

async Task QueryFilter()
{
    Console.WriteLine("ingrese una compañía de streaming");
    var streamingName = Console.ReadLine();
    var streamers = await dbContext.Streamers.Where(x => x.Nombre == streamingName).ToListAsync();
    foreach (var streamer in streamers)
    {
        Console.WriteLine($"{streamer.Id} - {streamer.Nombre}");
    }
    //var streamersPartial = await dbContext.Streamers.Where(x => x.Nombre.Contains(streamingName)).ToListAsync();
    var streamersPartial = await dbContext.Streamers.Where(x => EF.Functions.Like(x.Nombre, $"%{streamingName}%")).ToListAsync();
    foreach (var streamer in streamersPartial)
    {
        Console.WriteLine($"{streamer.Id} - {streamer.Nombre}");
    }
}

async Task QueryStreaming()
{
    var streamers = await dbContext.Streamers.ToListAsync();
    foreach (var streamer in streamers)
    {
        Console.WriteLine($"{streamer.Id} - {streamer.Nombre}");
    }
}

async Task AddNewRecords()
{
    Streamer streamer = new() { Nombre = "Disney", Url = "https://www.disney.com" };

    await dbContext.Streamers.AddAsync(streamer);
    await dbContext.SaveChangesAsync();

    var movies = new List<Video>
    {
        new Video{Nombre = "Iron Man", StreamerId = streamer.Id},
        new Video{Nombre = "los vengadores", StreamerId = streamer.Id},
        new Video{Nombre = "Tierra de Osos", StreamerId = streamer.Id}
    };

    await dbContext.Videos.AddRangeAsync(movies);
    await dbContext.SaveChangesAsync();
}