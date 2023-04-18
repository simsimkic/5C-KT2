namespace SIMS_Projekat.Serializer
{
    public interface ISerializable
    {
        uint Id { get; set; }
        bool IsDeleted { get; set; }
        string[] ToCSV();
        void FromCSV(string[] values);

    }
}
