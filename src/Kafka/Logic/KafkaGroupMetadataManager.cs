using System;
using System.IO;
using System.Text;

namespace Detectors.Kafka.Logic
{
    public class KafkaGroupMetadataManager
    {
        
/**
        private val OFFSET_COMMIT_KEY_SCHEMA = new Schema(new Field("group", STRING),
            new Field("topic", STRING),
            new Field("partition", INT32))

 * Messages stored for the group topic has versions for both the key and value fields. Key
 * version is used to indicate the type of the message (also to differentiate different types
 * of messages from being compacted together if they have the same field values); and value
 * version is used to evolve the messages within their data types:
 *
 * key version 0:       group consumption offset
 *    -> value version 0:       [offset, metadata, timestamp]
 *
 * key version 1:       group consumption offset
 *    -> value version 1:       [offset, metadata, commit_timestamp, expire_timestamp]
 *
 * key version 2:       group metadata
 *     -> value version 0:       [protocol_type, generation, protocol, leader, members]
 */
        
        public static void ReadMessage(byte[] key, byte[] value)
        {
            var keyStream = new MemoryStream(key);
            var valueStream = new MemoryStream(value);
            
            var keyVersion = ReadMessageKey(new BinaryReader(keyStream));
            if (keyVersion <= 1)
                ReadOffsetMessageValue(new BinaryReader(valueStream));
            else
                ReadGroupMessageValue(new BinaryReader(valueStream));
        }
        
        public static short ReadMessageKey(BinaryReader reader)
        {
            var version = ReadInt16(reader);
            switch (version)
            {
                case 0:
                case 1:
                    ReadOffsetCommitKey(reader);
                    break;
                    
                case 2:
                    ReadGroupMetadataKey(reader);
                    break;
            }

            return version;
        }

        public static void ReadOffsetMessageValue(BinaryReader reader)
        {
            var version = ReadInt16(reader);
            switch (version)
            {
                case 0:
                    ReadOffsetMessageValueV0(reader);
                    break;
                    
                case 1:
                    ReadOffsetMessageValueV1(reader);
                    break;
            }
        }

        public static void ReadGroupMessageValue(BinaryReader reader)
        {
            var version = ReadInt16(reader);
            switch (version)
            {
                case 0:
                    ReadGroupMessageValueV0(reader);
                    break;
                    
                case 1:
                    ReadGroupMessageValueV1(reader);
                    break;
            }
        }

        public static void ReadOffsetMessageValueV0(BinaryReader reader)
        {
            Console.WriteLine($"offset: {ReadInt64(reader)}");
            Console.WriteLine($"metadata: {ReadString(reader)}");
            Console.WriteLine($"timestamp: {ReadInt64(reader)}");
        }

        public static void ReadOffsetMessageValueV1(BinaryReader reader)
        {
            Console.WriteLine($"offset: {ReadInt64(reader)}");
            Console.WriteLine($"metadata: {ReadString(reader)}");
            Console.WriteLine($"commit_timestamp: {ReadInt64(reader)}");
            Console.WriteLine($"expire_timestamp: {ReadInt64(reader)}");
        }

        public static void ReadGroupMessageValueV0(BinaryReader reader)
        {
            Console.WriteLine($"protocol_type: {ReadString(reader)}");
            Console.WriteLine($"generation_key: {ReadInt32(reader)}");
            Console.WriteLine($"protocol: {ReadNullableString(reader)}");
            Console.WriteLine($"leader: {ReadNullableString(reader)}");

            var memberCount = ReadInt32(reader);
            Console.WriteLine($"member_count: {memberCount}");

            for (var i = 0; i < memberCount; i++)
            {
                Console.WriteLine($"  - Member [{i}]");
                Console.WriteLine($"    > member_id: {ReadString(reader)}");
                Console.WriteLine($"    > client_id: {ReadString(reader)}");
                Console.WriteLine($"    > client_host: {ReadString(reader)}");
                Console.WriteLine($"    > session_timeout: {ReadInt32(reader)}");
                Console.WriteLine($"    > subscription_key: {ReadBytes(reader)}");
                Console.WriteLine($"    > assignment_key: {ReadBytes(reader)}");
            }
        }
        
        public static void ReadGroupMessageValueV1(BinaryReader reader)
        {
            Console.WriteLine($"protocol_type: {ReadString(reader)}");
            Console.WriteLine($"generation_key: {ReadInt32(reader)}");
            Console.WriteLine($"protocol: {ReadNullableString(reader)}");
            Console.WriteLine($"leader: {ReadNullableString(reader)}");
            
            var memberCount = ReadInt32(reader);
            Console.WriteLine($"member_count: {memberCount}");

            for (var i = 0; i < memberCount; i++)
            {
                Console.WriteLine($"  - Member [{i}]");
                Console.WriteLine($"    > member_id: {ReadString(reader)}");
                Console.WriteLine($"    > client_id: {ReadString(reader)}");
                Console.WriteLine($"    > client_host: {ReadString(reader)}");
                Console.WriteLine($"    > rebalance_timeout: {ReadInt32(reader)}");
                Console.WriteLine($"    > session_timeout: {ReadInt32(reader)}");
                Console.WriteLine($"    > subscription_key: {ReadBytes(reader)}");
                Console.WriteLine($"    > assignment_key: {ReadBytes(reader)}");
            }
        }
        
        public static void ReadOffsetCommitKey(BinaryReader reader)
        {
            Console.WriteLine($"group: {ReadString(reader)}");
            Console.WriteLine($"topic: {ReadString(reader)}");
            Console.WriteLine($"partition: {ReadInt32(reader)}");
        }

        public static void ReadGroupMetadataKey(BinaryReader reader)
        {
            Console.WriteLine($"group: {ReadString(reader)}");
        }

        private static string ReadString(BinaryReader reader)
        {
            var length = ReadInt16(reader);
            return Encoding.UTF8.GetString(reader.ReadBytes(length));
        }

        private static string ReadNullableString(BinaryReader reader)
        {
            var length = ReadInt16(reader);
            return Encoding.UTF8.GetString(reader.ReadBytes(length));
        }

        private static string ReadBytes(BinaryReader reader)
        {
            var size = ReadInt32(reader);
            return Convert.ToBase64String(reader.ReadBytes(size));
        }
        
        private static int ReadInt32(BinaryReader reader)
        {
            var data = reader.ReadBytes(sizeof(int));
            Array.Reverse(data);
            return BitConverter.ToInt32(data, 0);
        }

        private static short ReadInt16(BinaryReader reader)
        {
            var data = reader.ReadBytes(sizeof(short));
            Array.Reverse(data);
            return BitConverter.ToInt16(data, 0);
        }

        private static long ReadInt64(BinaryReader reader)
        {
            var data = reader.ReadBytes(sizeof(long));
            Array.Reverse(data);
            return BitConverter.ToInt64(data, 0);
        }
    }
}