using System;

namespace TicketVendingMachine
{
    // Интерфейс состояния
    interface ITicketState
    {
        void SelectTicket();
        void InsertMoney();
        void CancelTransaction();
        void DispenseTicket();
    }

    // Контекст автомата
    class TicketMachine
    {
        public ITicketState IdleState { get; private set; }
        public ITicketState WaitingForMoneyState { get; private set; }
        public ITicketState MoneyReceivedState { get; private set; }
        public ITicketState TicketDispensedState { get; private set; }
        public ITicketState TransactionCanceledState { get; private set; }

        public ITicketState CurrentState { get; set; }

        public TicketMachine()
        {
            IdleState = new Idle(this);
            WaitingForMoneyState = new WaitingForMoney(this);
            MoneyReceivedState = new MoneyReceived(this);
            TicketDispensedState = new TicketDispensed(this);
            TransactionCanceledState = new TransactionCanceled(this);

            CurrentState = IdleState;
        }

        public void SelectTicket() => CurrentState.SelectTicket();
        public void InsertMoney() => CurrentState.InsertMoney();
        public void CancelTransaction() => CurrentState.CancelTransaction();
        public void DispenseTicket() => CurrentState.DispenseTicket();
    }

    // Состояния
    class Idle : ITicketState
    {
        private TicketMachine machine;
        public Idle(TicketMachine m) { machine = m; }
        public void SelectTicket() { Console.WriteLine("Билет выбран."); machine.CurrentState = machine.WaitingForMoneyState; }
        public void InsertMoney() { Console.WriteLine("Сначала выберите билет."); }
        public void CancelTransaction() { Console.WriteLine("Нет транзакции для отмены."); }
        public void DispenseTicket() { Console.WriteLine("Сначала выберите билет."); }
    }

    class WaitingForMoney : ITicketState
    {
        private TicketMachine machine;
        public WaitingForMoney(TicketMachine m) { machine = m; }
        public void SelectTicket() { Console.WriteLine("Билет уже выбран."); }
        public void InsertMoney() { Console.WriteLine("Деньги внесены."); machine.CurrentState = machine.MoneyReceivedState; }
        public void CancelTransaction() { Console.WriteLine("Транзакция отменена."); machine.CurrentState = machine.TransactionCanceledState; }
        public void DispenseTicket() { Console.WriteLine("Сначала внесите деньги."); }
    }

    class MoneyReceived : ITicketState
    {
        private TicketMachine machine;
        public MoneyReceived(TicketMachine m) { machine = m; }
        public void SelectTicket() { Console.WriteLine("Билет уже выбран."); }
        public void InsertMoney() { Console.WriteLine("Деньги уже внесены."); }
        public void CancelTransaction() { Console.WriteLine("Транзакция отменена."); machine.CurrentState = machine.TransactionCanceledState; }
        public void DispenseTicket() { Console.WriteLine("Билет выдан."); machine.CurrentState = machine.TicketDispensedState; }
    }

    class TicketDispensed : ITicketState
    {
        private TicketMachine machine;
        public TicketDispensed(TicketMachine m) { machine = m; }
        public void SelectTicket() { Console.WriteLine("Сначала завершите предыдущую транзакцию."); }
        public void InsertMoney() { Console.WriteLine("Сначала завершите предыдущую транзакцию."); }
        public void CancelTransaction() { Console.WriteLine("Транзакция завершена."); }
        public void DispenseTicket() { Console.WriteLine("Билет уже выдан."); machine.CurrentState = machine.IdleState; }
    }

    class TransactionCanceled : ITicketState
    {
        private TicketMachine machine;
        public TransactionCanceled(TicketMachine m) { machine = m; }
        public void SelectTicket() { Console.WriteLine("Транзакция отменена, возвращаемся в Idle."); machine.CurrentState = machine.IdleState; }
        public void InsertMoney() { Console.WriteLine("Транзакция отменена."); }
        public void CancelTransaction() { Console.WriteLine("Транзакция уже отменена."); }
        public void DispenseTicket() { Console.WriteLine("Невозможно выдать билет."); }
    }

    // Пример использования
    class Program
    {
        static void Main(string[] args)
        {
            TicketMachine machine = new TicketMachine();

            machine.SelectTicket();
            machine.InsertMoney();
            machine.DispenseTicket();

            machine.SelectTicket();
            machine.CancelTransaction();
        }
    }
}
