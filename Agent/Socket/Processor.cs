namespace Agent.Socket
{
    public class Processor(ProgramContext context)
    {
        private readonly Dispatcher dispatcher = new(context);

        public async Task Start()
        {
            var result = await context.Reader.Read();

            while (result != null)
            {
                await ProcessMessage(result);

                result = await context.Reader.Read();
            }
        }

        private async Task ProcessMessage(string message)
        {
            Console.WriteLine($"Received message: {message}");
            await dispatcher.Dispatch(message);
        }
    }
}