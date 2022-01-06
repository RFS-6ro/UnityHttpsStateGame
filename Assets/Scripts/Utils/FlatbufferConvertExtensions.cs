namespace Utils.Extensions
{
    public static class FlatbufferConvertExtensions
    {
        public static UnityEngine.Vector3 ToUnityVector3(this Serialization.Physics.Vector3 vector) =>
            new UnityEngine.Vector3(vector.X, vector.Y, vector.Z);

        public static FlatBuffers.Offset<Serialization.Physics.Vector3> ToFlatBufferVector3(this UnityEngine.Vector3 vector, FlatBuffers.FlatBufferBuilder builder) =>
            Serialization.Physics.Vector3.CreateVector3(builder, vector.x, vector.y, vector.z);

        public static UnityEngine.Quaternion ToUnityQuaternion(this Serialization.Physics.Quaternion quaternion) =>
            new UnityEngine.Quaternion(quaternion.X, quaternion.Y, quaternion.Z, quaternion.W);

        public static FlatBuffers.Offset<Serialization.Physics.Quaternion> ToFlatBufferQuaternion(this UnityEngine.Quaternion quaternion, FlatBuffers.FlatBufferBuilder builder) =>
            Serialization.Physics.Quaternion.CreateQuaternion(builder, quaternion.x, quaternion.y, quaternion.z, quaternion.w);
    }
}
