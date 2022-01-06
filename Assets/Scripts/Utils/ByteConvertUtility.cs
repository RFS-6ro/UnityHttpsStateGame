using System;
using System.Text;

namespace Utils.Network
{
	public static class ByteConvertUtility
	{
		/// <summary>Reads a short from the packet.</summary>
		public static short ReadShort(this byte[] buffer, int readPosition = 0)
		{
			if (buffer.Length > readPosition &&
			    buffer.Length - readPosition > 2)
			{
				// If there are unread bytes
				return BitConverter.ToInt16(buffer, readPosition); // Convert the bytes to a short
			}

			throw new Exception("Could not read value of type 'short'!");
		}

		/// <summary>Reads an int from the packet.</summary>
		public static int ReadInt(this byte[] buffer, int readPosition = 0)
		{
			if (buffer.Length > readPosition &&
			    buffer.Length - readPosition > 4)
			{
				// If there are unread bytes
				return BitConverter.ToInt32(buffer, readPosition); // Convert the bytes to an int
			}

			throw new Exception("Could not read value of type 'int'!");
		}

		/// <summary>Reads a long from the packet.</summary>
		public static long ReadLong(this byte[] buffer, int readPosition = 0)
		{
			if (buffer.Length > readPosition &&
			    buffer.Length - readPosition > 8)
			{
				// If there are unread bytes
				return BitConverter.ToInt64(buffer, readPosition); // Convert the bytes to a long
			}

			throw new Exception("Could not read value of type 'long'!");
		}

		/// <summary>Reads a float from the packet.</summary>
		public static float ReadFloat(this byte[] buffer, int readPosition = 0)
		{
			if (buffer.Length > readPosition &&
			    buffer.Length - readPosition > 4)
			{
				// If there are unread bytes
				return BitConverter.ToSingle(buffer, readPosition); // Convert the bytes to a float
			}

			throw new Exception("Could not read value of type 'float'!");
		}

		/// <summary>Reads a bool from the packet.</summary>
		public static bool ReadBool(this byte[] buffer, int readPosition = 0)
		{
			if (buffer.Length > readPosition)
			{
				// If there are unread bytes
				return BitConverter.ToBoolean(buffer, readPosition); // Convert the bytes to a bool
			}

			throw new Exception("Could not read value of type 'bool'!");
		}

		/// <summary>Reads a string from the packet.</summary>
		public static string ReadString(this byte[] buffer, int readPosition = 0, int length = 0)
		{
			try
			{
				// Convert the bytes to a string
				return length == 0
					? Encoding.ASCII.GetString(buffer)
					: Encoding.ASCII.GetString(buffer, readPosition, length);
			}
			catch
			{
				throw new Exception("Could not read value of type 'string'!");
			}
		}
	}
}
