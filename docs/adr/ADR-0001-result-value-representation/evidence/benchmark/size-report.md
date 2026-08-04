# Result state-width size report

Measured with `Unsafe.SizeOf<T>()` on .NET 10.0.10, Windows x64.

| `TValue` | `TFailure` | Byte state | Int state |
| --- | --- | ---: | ---: |
| `int` | byte-backed enum | 8 bytes | 12 bytes |
| `int` | int-backed enum | 12 bytes | 12 bytes |
| `object` | byte-backed enum | 16 bytes | 16 bytes |
| `object` | int-backed enum | 16 bytes | 16 bytes |
| 64-byte value | byte-backed enum | 72 bytes | 72 bytes |
| 64-byte value | int-backed enum | 72 bytes | 72 bytes |

The byte discriminator reduced only the measured small-value/byte-failure closed generic. Alignment
removed the nominal field-width difference from every other measured shape.
