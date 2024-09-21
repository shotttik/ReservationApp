namespace Domain.Entities
{
    public class HashingAlgorithm
    {
        public required int ID { get; set; }
        public required string AlgorithmName { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public ICollection<UserLoginData> UserLoginDatas { get; } = [];

    }
}
