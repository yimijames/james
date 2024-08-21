using System;
using RabbitMQ.Client;
using System.Text;

class TemperatureSensor
{
    public static void Main(string[] args)
    {
        // Simula uma leitura de temperatura
        double temperaturaAtual = ObterTemperatura();

        if (temperaturaAtual >= 35)
        {
            EnviarMensagem($"Alerta: Temperatura alta! {temperaturaAtual}°C");
        }
    }

    private static double ObterTemperatura()
    {
        // Simulação de leitura da temperatura
        Random rand = new Random();
        return rand.Next(30, 40);
    }

    private static void EnviarMensagem(string mensagem)
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: "fila_temperatura",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var body = Encoding.UTF8.GetBytes(mensagem);

            channel.BasicPublish(exchange: "",
                                 routingKey: "fila_temperatura",
                                 basicProperties: null,
                                 body: body);
            Console.WriteLine(" [x] Enviado {0}", mensagem);
        }
    }
}
