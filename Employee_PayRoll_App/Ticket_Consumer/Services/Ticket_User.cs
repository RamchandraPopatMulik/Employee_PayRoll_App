using Employee_Payroll_Model;
using MassTransit;
using System.Threading.Tasks;

namespace Ticket_Consumer.Services
{
    public class Ticket_User : IConsumer<User_Ticket>
    {
        public async Task Consume(ConsumeContext<User_Ticket> context)
        {
            var data = context.Message;
            //Validate the Ticket Data
            //Store to Database
            //Notify the user via Email / SMS
        }
    }
}
