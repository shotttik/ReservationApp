namespace Domain.Entities
{
    public class HashingAlgorithm
    {
        public int ID { get; set; }
        public string AlgorithmName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public ICollection<UserLoginData> UserLoginDatas { get; } = [];

    }
}
