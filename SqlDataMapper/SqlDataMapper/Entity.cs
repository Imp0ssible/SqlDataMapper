namespace SqlDataMapper
{
    public class Entity
    {
        [DbColumn(ColumnName = "id")]
        public int Id { get; set; }

        [DbColumn(ColumnName = "my_name")]
        public string Name { get; set; }

        [DbColumn(ColumnName = "our_description")]
        public string Description { get; set; }

        [DbColumn(ColumnName = "they_price")]
        public decimal? Price { get; set; }

        [DbColumn(ColumnName = "a_type")]
        public int? Type { get; set; }

        [DbColumn(ColumnName = "on_fly")]
        public byte? Fly { get; set; }

        [DbColumn(ColumnName = "is_active")]
        public bool IsActive { get; set; }

        [DbColumn(ColumnName = "is_deleted")]
        public bool? IsDeleted { get; set; }

        [DbColumn(ColumnName = "enum_type")]
        public EnumType MyEnumType { get; set; }
    }
}
