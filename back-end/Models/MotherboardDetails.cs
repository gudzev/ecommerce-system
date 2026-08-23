using Microsoft.Data.SqlClient;

namespace Backend.Models
{
    public class MotherboardDetails
    {
        public string? socket { get; set; }
        public string? ramType { get; set; }
        public string? chipset { get; set; }
        public bool? wifi { get; set; }
        public bool? bluetooth { get; set; }
        public int? ramSlots { get; set; }
        public int? m2Slots { get; set; }
        public int? sataSlots { get; set; }
        public int? pcieSlots { get; set; }
        public string? size { get; set; }

        public async Task<MotherboardDetails> getDetails(string connectionString, int productId)
        {
            MotherboardDetails details;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                string query = @"SELECT * FROM motherboards WHERE product_id = @productId";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@productId", productId);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            details = new MotherboardDetails();
                            details.socket = reader["socket"].ToString();
                            details.size = reader["size"].ToString();
                            details.ramType = reader["ram_type"].ToString();
                            details.chipset = reader["chipset"].ToString();
                            details.ramSlots = Convert.ToInt32(reader["ram_slots"]);
                            details.m2Slots = Convert.ToInt32(reader["m2_slots"]);
                            details.sataSlots = Convert.ToInt32(reader["sata_slots"]);
                            details.pcieSlots = Convert.ToInt32(reader["pcie_slots"]);
                            details.wifi = Convert.ToBoolean(reader["wi_fi"]);
                            details.bluetooth = Convert.ToBoolean(reader["bluetooth"]);

                            return details;
                        }
                    }
                }
            }
            return null;
        }

        public async void postDetails(string connectionString, Product p)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                string query = @"INSERT INTO motherboards(product_id, socket, ram_type, chipset, bluetooth, wi_fi, ram_slots, sata_slots, m2_slots, pcie_slots, size)
                                 VALUES(@product_id, @socket, @ram_type, @chipset, @bluetooth, @wi_fi, @ram_slots, @sata_slots, @m2_slots, @pcie_slots, @size)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@product_id", p.id);
                    command.Parameters.AddWithValue("@socket", p?.motherboardDetails?.socket);
                    command.Parameters.AddWithValue("@ram_type", p?.motherboardDetails?.ramType);
                    command.Parameters.AddWithValue("@chipset", p?.motherboardDetails?.chipset);
                    command.Parameters.AddWithValue("@bluetooth", p?.motherboardDetails?.bluetooth);
                    command.Parameters.AddWithValue("@wi_fi", p?.motherboardDetails?.wifi);
                    command.Parameters.AddWithValue("@ram_slots", p?.motherboardDetails?.ramSlots);
                    command.Parameters.AddWithValue("@sata_slots", p?.motherboardDetails?.sataSlots);
                    command.Parameters.AddWithValue("@pcie_slots", p?.motherboardDetails?.pcieSlots);
                    command.Parameters.AddWithValue("@size", p?.motherboardDetails?.size);
                    command.Parameters.AddWithValue("@m2_slots", p?.motherboardDetails?.m2Slots);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async void putDetails(string connectionString, Product p)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                string query = @"UPDATE motherboards
                                 SET product_id = @product_id, socket = @socket, ram_type = @ram_type, chipset = @chipset, bluetooth = @bluetooth,
                                     wi_fi = @wi_fi, ram_slots = @ram_slots, sata_slots =  @sata_slots, m2_slots = @m2_slots, pcie_slots =  @pcie_slots, size = @size 
                                 WHERE product_id = @product_id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@product_id", p.id);
                    command.Parameters.AddWithValue("@socket", p?.motherboardDetails?.socket);
                    command.Parameters.AddWithValue("@ram_type", p?.motherboardDetails?.ramType);
                    command.Parameters.AddWithValue("@chipset", p?.motherboardDetails?.chipset);
                    command.Parameters.AddWithValue("@bluetooth", p?.motherboardDetails?.bluetooth);
                    command.Parameters.AddWithValue("@wi_fi", p?.motherboardDetails?.wifi);
                    command.Parameters.AddWithValue("@ram_slots", p?.motherboardDetails?.ramSlots);
                    command.Parameters.AddWithValue("@sata_slots", p?.motherboardDetails?.sataSlots);
                    command.Parameters.AddWithValue("@pcie_slots", p?.motherboardDetails?.pcieSlots);
                    command.Parameters.AddWithValue("@size", p?.motherboardDetails?.size);
                    command.Parameters.AddWithValue("@m2_slots", p?.motherboardDetails?.m2Slots);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
