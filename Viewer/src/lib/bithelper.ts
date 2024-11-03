import { ByteStream } from "./stream.js";
import { Vec3 } from "./world.js";

declare global {
    interface Math {
        clamp(value: number, min: number, max: number): number;
        clamp01(value: number): number;
        repeat(t: number, length: number): number;
        deltaAngle(current: number, target: number): number;
        deg2rad: number;
        rad2deg: number;
    }
}

Math.deg2rad = Math.PI / 180.0;
Math.rad2deg = 180.0 / Math.PI;

Math.clamp = function (value, min, max) {
    return Math.min(max, Math.max(value, min));
};
Math.clamp01 = function (value) {
    return Math.clamp(value, 0, 1);
};

// https://github.com/Unity-Technologies/UnityCsReference/blob/e740821767d2290238ea7954457333f06e952bad/Runtime/Export/Math/Mathf.cs#L357
Math.repeat = function(t, length) {
    return Math.clamp(t - Math.floor(t / length) * length, 0, length);
};
Math.deltaAngle = function(current, target) {
    let delta = Math.repeat((target - current), 360.0);
    if (delta > 180.0) {
        delta -= 360.0;
    }
    return delta;
};

export const littleEndian: boolean = (() => {
    const buffer = new ArrayBuffer(2);
    new DataView(buffer).setInt16(0, 256, true /* littleEndian */);
    // Int16Array uses the platform's endianness.
    return new Int16Array(buffer)[0] === 256;
})();

export type Type = "byte" | "ushort" | "uint" | "ulong" | "short" | "int" | "long" | "float" | "single" | "half" | "vector" | "halfVector" | "quaternion" | "halfQuaternion";
export function sizeof(type: Type): number {
    switch(type) {
    case "byte": return 1;
    case "short":
    case "ushort": 
    case "half": return 2;
    case "int":
    case "uint": 
    case "float":
    case "single": return 4;
    case "long":
    case "ulong": return 8;
    case "vector": return 12;
    case "halfVector": return 6;
    case "quaternion": return 16;
    case "halfQuaternion": return 8;
    default: throw new Error("Invalid type.");
    }
}

export function readByte(stream: ByteStream): number {
    return stream.view.getUint8(stream.index++);
}

export function readBool(stream: ByteStream): boolean {
    return readByte(stream) != 0;
}

export function readString(stream: ByteStream, length?: number): string {
    if (length === undefined) length = readUShort(stream);
    const start = stream.view.byteOffset + stream.index;
    const end = stream.view.byteOffset + (stream.index += length);
    return new TextDecoder().decode(stream.view.buffer.slice(start, end));
}

export function readULong(stream: ByteStream): bigint {
    const sizeof = 8;
    const index = stream.index;
    stream.index += sizeof;
    return stream.view.getBigUint64(index, littleEndian);
}
export function readUInt(stream: ByteStream): number {
    const sizeof = 4;
    const index = stream.index;
    stream.index += sizeof;
    return stream.view.getUint32(index, littleEndian);
}
export function readUShort(stream: ByteStream): number {
    const sizeof = 2;
    const index = stream.index;
    stream.index += sizeof;
    return stream.view.getUint16(index, littleEndian);
}

export function readLong(stream: ByteStream): bigint {
    const sizeof = 8;
    const index = stream.index;
    stream.index += sizeof;
    return stream.view.getBigInt64(index, littleEndian);
}
export function readInt(stream: ByteStream): number {
    const sizeof = 4;
    const index = stream.index;
    stream.index += sizeof;
    return stream.view.getInt32(index, littleEndian);
}
export function readShort(stream: ByteStream): number {
    const sizeof = 2;
    const index = stream.index;
    stream.index += sizeof;
    return stream.view.getInt16(index, littleEndian);
}

export function readHalf(stream: ByteStream): number {
    const ushort = readUShort(stream);

    // Create a 32 bit DataView to store the input
    const arr = new ArrayBuffer(4);
    const dv = new DataView(arr);

    // Set the Float16 into the last 16 bits of the dataview
    // So our dataView is [00xx]
    dv.setUint16(2, ushort);

    // Get all 32 bits as a 32 bit integer
    // (JS bitwise operations are performed on 32 bit signed integers)
    const asInt32 = dv.getInt32(0);

    // All bits aside from the sign
    let rest = asInt32 & 0x7fff;
    // Sign bit
    let sign = asInt32 & 0x8000;
    // Exponent bits
    const exponent = asInt32 & 0x7c00;

    // Shift the non-sign bits into place for a 32 bit Float
    rest <<= 13;
    // Shift the sign bit into place for a 32 bit Float
    sign <<= 16;

    // Adjust bias
    // https://en.wikipedia.org/wiki/Half-precision_floating-point_format#Exponent_encoding
    rest += 0x38000000;
    // Denormals-as-zero
    rest = (exponent === 0 ? 0 : rest);
    // Re-insert sign bit
    rest |= sign;

    // Set the adjusted float32 (stored as int32) back into the dataview
    dv.setInt32(0, rest);

    // Get it back out as a float32 (which js will convert to a Number)
    return dv.getFloat32(0);
}
export function readFloat(stream: ByteStream): number {
    const sizeof = 4;
    const index = stream.index;
    stream.index += sizeof;
    return stream.view.getFloat32(index, littleEndian);
}
export function readVector(stream: ByteStream): Vec3 {
    return new Vec3(readInt(stream), readInt(stream), readInt(stream));
}
export function readHalfVector(stream: ByteStream): Vec3 {
    return new Vec3(readInt(stream), readInt(stream), readInt(stream));
}

export function readUShortArray(stream: ByteStream, length: number): number[] {
    const array = new Array(length);
    for (let i = 0; i < length; ++i)
        array[i] = readUShort(stream);
    return array;
}
export function readVectorArray(stream: ByteStream, length: number): Vec3[] {
    const array = new Array<Vec3>(length);
    for (let i = 0; i < length; ++i) {
        array[i] = readVector(stream);
    }
    return array;
}
export function readVectorArrayAsFloat32(stream: ByteStream, length: number): Float32Array {
    length *= 3;
    const array = new Array<number>(length);
    for (let i = 0; i < length;) {
        array[i++] = -readFloat(stream);
        array[i++] = readFloat(stream);
        array[i++] = readFloat(stream);
    }
    return new Float32Array(array);
}
export function readFloat32Array(stream: ByteStream, length: number): Float32Array {
    const array = new Array<number>(length);
    for (let i = 0; i < length; ++i) {
        array[i] = readFloat(stream);
    }
    return new Float32Array(array);
}

function reserve(numBytes: number, stream: ByteStream) {
    const size = stream.index + numBytes;
    let capacity = stream.view.byteLength;
    while (size > capacity) {
        if (capacity === 0) capacity = 1;
        capacity *= 2;
    }
    if (capacity !== stream.view.byteLength) {
        const newBuffer = new Uint8Array(capacity);
        newBuffer.set(stream.bytes);
        stream.assign(newBuffer);
    }
}

export function GetBytes(stream: ByteStream) {
    const size = stream.index;
    const capacity = stream.view.byteLength;
    if (size !== capacity) {
        const newBuffer = new Uint8Array(capacity);
        newBuffer.set(stream.bytes);
        stream.assign(newBuffer);
    }
    return stream.bytes;
}

export function writeByte(byte: number, stream: ByteStream) {
    if (byte < 0 || byte > 255) throw new TypeError("Value is not of type 'byte'.");
    const sizeof = 1;
    reserve(sizeof, stream);
    stream.view.setUint8(stream.index, byte);
    stream.index += sizeof;
}

export function writeBool(bool: boolean, stream: ByteStream) {
    writeByte(bool ? 1 : 0, stream);
}

export function writeBytes(bytes: Uint8Array, stream: ByteStream) {
    writeUShort(bytes.byteLength, stream);
    reserve(bytes.byteLength, stream);
    for (let i = 0; i < bytes.byteLength; ++i) {
        writeByte(bytes[i], stream);
    }
}

const textEncoder = new TextEncoder();
export function writeString(message: string, stream: ByteStream) {
    writeBytes(textEncoder.encode(message), stream);
}

export function writeULong(ulong: bigint, stream: ByteStream) {
    const sizeof = 8;
    reserve(sizeof, stream);
    stream.view.setBigUint64(stream.index, ulong, littleEndian);
    stream.index += sizeof;
}
export function writeUInt(uint: number, stream: ByteStream) {
    const sizeof = 4;
    reserve(sizeof, stream);
    stream.view.setUint32(stream.index, uint, littleEndian);
    stream.index += sizeof;
}
export function writeUShort(ushort: number, stream: ByteStream) {
    if (ushort < 0 || ushort > 65535) throw new TypeError("Value is not of type 'ushort'.");
    const sizeof = 2;
    reserve(sizeof, stream);
    stream.view.setUint16(stream.index, ushort, littleEndian);
    stream.index += sizeof;
}

export function writeLong(long: bigint, stream: ByteStream) {
    const sizeof = 8;
    reserve(sizeof, stream);
    stream.view.setBigInt64(stream.index, long, littleEndian);
    stream.index += sizeof;
}
export function writeInt(int: number, stream: ByteStream) {
    const sizeof = 4;
    reserve(sizeof, stream);
    stream.view.setInt32(stream.index, int, littleEndian);
    stream.index += sizeof;
}
export function writeShort(short: number, stream: ByteStream) {
    if (short < 0 || short > 65535) throw new TypeError("Value is not of type 'short'.");
    const sizeof = 2;
    reserve(sizeof, stream);
    stream.view.setInt16(stream.index, short, littleEndian);
    stream.index += sizeof;
}

function ToInt(expr: boolean) {
    return expr ? 1 : 0;
}

export function writeHalf(float: number, stream: ByteStream) {
    // Create a 32 bit DataView to store the input
    const arr = new ArrayBuffer(4);
    const dv = new DataView(arr);
    dv.setFloat32(0, float);
    const uint = dv.getUint32(0);

    const b = uint + 0x00001000; // round-to-nearest-even: add last bit after truncated mantissa
    const e = (b & 0x7F800000) >> 23; // exponent
    const m = b & 0x007FFFFF; // mantissa; in line below: 0x007FF000 = 0x00800000-0x00001000 = decimal indicator flag - initial rounding

    writeUShort((b & 0x80000000) >> 16 | ToInt(e > 112) * ((((e - 112) << 10) & 0x7C00) | m >> 13) | (ToInt(e < 113) & ToInt(e > 101)) * ((((0x007FF000 + m) >> (125 - e)) + 1) >> 1) | ToInt(e > 143) * 0x7FFF, stream);
}
export function writeFloat(float: number, stream: ByteStream) {
    const sizeof = 4;
    reserve(sizeof, stream);
    stream.view.setFloat32(stream.index, float, littleEndian);
    stream.index += sizeof;
}