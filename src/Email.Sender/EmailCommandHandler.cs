namespace EaiGuy.Email.Sender
{
    using Messages.Commands;
    using NServiceBus;

    class EmailCommandHandler : IHandleMessages<EmailCommand>
    {
        public void Handle(EmailCommand cmd)
        {
            new EmailProvider(new EmailSettings()).SendEmail(cmd);
        }
    }
}