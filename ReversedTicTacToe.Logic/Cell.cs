namespace ReversedTicTacToe.Logic
{
    public class Cell
    {
        public Cell(GameLogic.eSign i_Value)
        {
            m_Value = i_Value;
            this.Available = true;
        }

        public GameLogic.eSign Value
        {
            get
            {
                return m_Value;
            }

            set
            {
                m_Value = value;
                Available = value == GameLogic.eSign.Empty;
            }
        }

        public bool Available { get; private set; }

        private GameLogic.eSign m_Value;
    }
}