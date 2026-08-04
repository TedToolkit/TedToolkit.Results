## .NET Framework 4.8.1 (4.8.9337.0), X64 RyuJIT VectorSize=256 (Job: DefaultJob)

```assembly
; TedToolkit.Results.Representation.Benchmarks.StateWidthBenchmarks.ByteStateWithByteFailure()
       push      r14
       push      rdi
       push      rsi
       push      rbp
       push      rbx
       sub       rsp,20
       mov       rsi,rcx
       xor       eax,eax
       xor       edi,edi
       mov       rcx,[rsi+8]
       cmp       dword ptr [rcx+8],0
       jle       short M00_L03
M00_L00:
       mov       rcx,[rsi+8]
       cmp       edi,[rcx+8]
       jae       short M00_L04
       movsxd    rdx,edi
       lea       rbx,[rcx+rdx*8+10]
       mov       ebp,eax
       mov       rcx,rbx
       call      TedToolkit.Results.Representation.Benchmarks.ByteStateResult`2[[System.Int32, mscorlib],[TedToolkit.Results.Representation.Benchmarks.ByteFailure, TedToolkit.Results.Representation.Benchmarks]].get_IsSuccess()
       test      al,al
       jne       short M00_L01
       mov       rcx,rbx
       call      TedToolkit.Results.Representation.Benchmarks.ByteStateResult`2[[System.Int32, mscorlib],[TedToolkit.Results.Representation.Benchmarks.ByteFailure, TedToolkit.Results.Representation.Benchmarks]].get_FailureReason()
       movzx     r14d,al
       jmp       short M00_L02
M00_L01:
       mov       rcx,rbx
       call      TedToolkit.Results.Representation.Benchmarks.ByteStateResult`2[[System.Int32, mscorlib],[TedToolkit.Results.Representation.Benchmarks.ByteFailure, TedToolkit.Results.Representation.Benchmarks]].get_Value()
       mov       r14d,eax
M00_L02:
       lea       eax,[r14+rbp]
       inc       edi
       mov       rdx,[rsi+8]
       cmp       [rdx+8],edi
       jg        short M00_L00
M00_L03:
       add       rsp,20
       pop       rbx
       pop       rbp
       pop       rsi
       pop       rdi
       pop       r14
       ret
M00_L04:
       call      CORINFO_HELP_RNGCHKFAIL
       int       3
; Total bytes of code 115
```
```assembly
; TedToolkit.Results.Representation.Benchmarks.ByteStateResult`2[[System.Int32, mscorlib],[TedToolkit.Results.Representation.Benchmarks.ByteFailure, TedToolkit.Results.Representation.Benchmarks]].get_IsSuccess()
       push      rsi
       sub       rsp,20
       movzx     eax,byte ptr [rcx+1]
       cmp       eax,1
       je        short M01_L01
       cmp       eax,2
       jne       short M01_L02
       xor       eax,eax
M01_L00:
       add       rsp,20
       pop       rsi
       ret
M01_L01:
       mov       eax,1
       jmp       short M01_L00
M01_L02:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rsi,rax
       mov       rcx,rsi
       call      qword ptr [7FFE7EA43980]; System.InvalidOperationException..ctor()
       mov       rcx,rsi
       call      CORINFO_HELP_THROW
       int       3
; Total bytes of code 70
```
```assembly
; TedToolkit.Results.Representation.Benchmarks.ByteStateResult`2[[System.Int32, mscorlib],[TedToolkit.Results.Representation.Benchmarks.ByteFailure, TedToolkit.Results.Representation.Benchmarks]].get_FailureReason()
       push      rsi
       sub       rsp,20
       cmp       byte ptr [rcx+1],2
       jne       short M02_L00
       movzx     eax,byte ptr [rcx]
       add       rsp,20
       pop       rsi
       ret
M02_L00:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rsi,rax
       mov       rcx,rsi
       call      qword ptr [7FFE7EA43980]; System.InvalidOperationException..ctor()
       mov       rcx,rsi
       call      CORINFO_HELP_THROW
       int       3
; Total bytes of code 56
```
```assembly
; TedToolkit.Results.Representation.Benchmarks.ByteStateResult`2[[System.Int32, mscorlib],[TedToolkit.Results.Representation.Benchmarks.ByteFailure, TedToolkit.Results.Representation.Benchmarks]].get_Value()
       push      rsi
       sub       rsp,20
       cmp       byte ptr [rcx+1],1
       jne       short M03_L00
       mov       eax,[rcx+4]
       add       rsp,20
       pop       rsi
       ret
M03_L00:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rsi,rax
       mov       rcx,rsi
       call      qword ptr [7FFE7EA43980]; System.InvalidOperationException..ctor()
       mov       rcx,rsi
       call      CORINFO_HELP_THROW
       int       3
; Total bytes of code 56
```

## .NET Framework 4.8.1 (4.8.9337.0), X64 RyuJIT VectorSize=256 (Job: DefaultJob)

```assembly
; TedToolkit.Results.Representation.Benchmarks.StateWidthBenchmarks.IntStateWithByteFailure()
       push      r14
       push      rdi
       push      rsi
       push      rbp
       push      rbx
       sub       rsp,20
       mov       rsi,rcx
       xor       eax,eax
       xor       edi,edi
       mov       rcx,[rsi+18]
       cmp       dword ptr [rcx+8],0
       jle       short M00_L03
M00_L00:
       mov       rcx,[rsi+18]
       cmp       edi,[rcx+8]
       jae       short M00_L04
       movsxd    rdx,edi
       lea       rdx,[rdx+rdx*2]
       lea       rbx,[rcx+rdx*4+10]
       mov       ebp,eax
       mov       rcx,rbx
       call      TedToolkit.Results.Representation.Benchmarks.IntStateResult`2[[System.Int32, mscorlib],[TedToolkit.Results.Representation.Benchmarks.ByteFailure, TedToolkit.Results.Representation.Benchmarks]].get_IsSuccess()
       test      al,al
       jne       short M00_L01
       mov       rcx,rbx
       call      TedToolkit.Results.Representation.Benchmarks.IntStateResult`2[[System.Int32, mscorlib],[TedToolkit.Results.Representation.Benchmarks.ByteFailure, TedToolkit.Results.Representation.Benchmarks]].get_FailureReason()
       movzx     r14d,al
       jmp       short M00_L02
M00_L01:
       mov       rcx,rbx
       call      TedToolkit.Results.Representation.Benchmarks.IntStateResult`2[[System.Int32, mscorlib],[TedToolkit.Results.Representation.Benchmarks.ByteFailure, TedToolkit.Results.Representation.Benchmarks]].get_Value()
       mov       r14d,eax
M00_L02:
       lea       eax,[r14+rbp]
       inc       edi
       mov       rdx,[rsi+18]
       cmp       [rdx+8],edi
       jg        short M00_L00
M00_L03:
       add       rsp,20
       pop       rbx
       pop       rbp
       pop       rsi
       pop       rdi
       pop       r14
       ret
M00_L04:
       call      CORINFO_HELP_RNGCHKFAIL
       int       3
; Total bytes of code 119
```
```assembly
; TedToolkit.Results.Representation.Benchmarks.IntStateResult`2[[System.Int32, mscorlib],[TedToolkit.Results.Representation.Benchmarks.ByteFailure, TedToolkit.Results.Representation.Benchmarks]].get_IsSuccess()
       push      rsi
       sub       rsp,20
       mov       eax,[rcx+4]
       cmp       eax,1
       je        short M01_L01
       cmp       eax,2
       jne       short M01_L02
       xor       eax,eax
M01_L00:
       add       rsp,20
       pop       rsi
       ret
M01_L01:
       mov       eax,1
       jmp       short M01_L00
M01_L02:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rsi,rax
       mov       rcx,rsi
       call      qword ptr [7FFE7EA43980]; System.InvalidOperationException..ctor()
       mov       rcx,rsi
       call      CORINFO_HELP_THROW
       int       3
; Total bytes of code 69
```
```assembly
; TedToolkit.Results.Representation.Benchmarks.IntStateResult`2[[System.Int32, mscorlib],[TedToolkit.Results.Representation.Benchmarks.ByteFailure, TedToolkit.Results.Representation.Benchmarks]].get_FailureReason()
       push      rsi
       sub       rsp,20
       cmp       dword ptr [rcx+4],2
       jne       short M02_L00
       movzx     eax,byte ptr [rcx]
       add       rsp,20
       pop       rsi
       ret
M02_L00:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rsi,rax
       mov       rcx,rsi
       call      qword ptr [7FFE7EA43980]; System.InvalidOperationException..ctor()
       mov       rcx,rsi
       call      CORINFO_HELP_THROW
       int       3
; Total bytes of code 56
```
```assembly
; TedToolkit.Results.Representation.Benchmarks.IntStateResult`2[[System.Int32, mscorlib],[TedToolkit.Results.Representation.Benchmarks.ByteFailure, TedToolkit.Results.Representation.Benchmarks]].get_Value()
       push      rsi
       sub       rsp,20
       cmp       dword ptr [rcx+4],1
       jne       short M03_L00
       mov       eax,[rcx+8]
       add       rsp,20
       pop       rsi
       ret
M03_L00:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rsi,rax
       mov       rcx,rsi
       call      qword ptr [7FFE7EA43980]; System.InvalidOperationException..ctor()
       mov       rcx,rsi
       call      CORINFO_HELP_THROW
       int       3
; Total bytes of code 56
```

## .NET Framework 4.8.1 (4.8.9337.0), X64 RyuJIT VectorSize=256 (Job: DefaultJob)

```assembly
; TedToolkit.Results.Representation.Benchmarks.StateWidthBenchmarks.ByteStateWithIntFailure()
       push      rdi
       push      rsi
       push      rbp
       push      rbx
       sub       rsp,28
       mov       rsi,rcx
       xor       eax,eax
       xor       edi,edi
       mov       rcx,[rsi+10]
       cmp       dword ptr [rcx+8],0
       jle       short M00_L03
M00_L00:
       mov       rcx,[rsi+10]
       cmp       edi,[rcx+8]
       jae       short M00_L04
       movsxd    rdx,edi
       lea       rdx,[rdx+rdx*2]
       lea       rbx,[rcx+rdx*4+10]
       mov       ebp,eax
       mov       rcx,rbx
       call      TedToolkit.Results.Representation.Benchmarks.ByteStateResult`2[[System.Int32, mscorlib],[TedToolkit.Results.Representation.Benchmarks.IntFailure, TedToolkit.Results.Representation.Benchmarks]].get_IsSuccess()
       test      al,al
       jne       short M00_L01
       mov       rcx,rbx
       call      TedToolkit.Results.Representation.Benchmarks.ByteStateResult`2[[System.Int32, mscorlib],[TedToolkit.Results.Representation.Benchmarks.IntFailure, TedToolkit.Results.Representation.Benchmarks]].get_FailureReason()
       jmp       short M00_L02
M00_L01:
       mov       rcx,rbx
       call      TedToolkit.Results.Representation.Benchmarks.ByteStateResult`2[[System.Int32, mscorlib],[TedToolkit.Results.Representation.Benchmarks.IntFailure, TedToolkit.Results.Representation.Benchmarks]].get_Value()
M00_L02:
       add       eax,ebp
       inc       edi
       mov       rdx,[rsi+10]
       cmp       [rdx+8],edi
       jg        short M00_L00
M00_L03:
       add       rsp,28
       pop       rbx
       pop       rbp
       pop       rsi
       pop       rdi
       ret
M00_L04:
       call      CORINFO_HELP_RNGCHKFAIL
       int       3
; Total bytes of code 106
```
```assembly
; TedToolkit.Results.Representation.Benchmarks.ByteStateResult`2[[System.Int32, mscorlib],[TedToolkit.Results.Representation.Benchmarks.IntFailure, TedToolkit.Results.Representation.Benchmarks]].get_IsSuccess()
       push      rsi
       sub       rsp,20
       movzx     eax,byte ptr [rcx+4]
       cmp       eax,1
       je        short M01_L01
       cmp       eax,2
       jne       short M01_L02
       xor       eax,eax
M01_L00:
       add       rsp,20
       pop       rsi
       ret
M01_L01:
       mov       eax,1
       jmp       short M01_L00
M01_L02:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rsi,rax
       mov       rcx,rsi
       call      qword ptr [7FFE7EA43980]; System.InvalidOperationException..ctor()
       mov       rcx,rsi
       call      CORINFO_HELP_THROW
       int       3
; Total bytes of code 70
```
```assembly
; TedToolkit.Results.Representation.Benchmarks.ByteStateResult`2[[System.Int32, mscorlib],[TedToolkit.Results.Representation.Benchmarks.IntFailure, TedToolkit.Results.Representation.Benchmarks]].get_FailureReason()
       push      rsi
       sub       rsp,20
       cmp       byte ptr [rcx+4],2
       jne       short M02_L00
       mov       eax,[rcx]
       add       rsp,20
       pop       rsi
       ret
M02_L00:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rsi,rax
       mov       rcx,rsi
       call      qword ptr [7FFE7EA43980]; System.InvalidOperationException..ctor()
       mov       rcx,rsi
       call      CORINFO_HELP_THROW
       int       3
; Total bytes of code 55
```
```assembly
; TedToolkit.Results.Representation.Benchmarks.ByteStateResult`2[[System.Int32, mscorlib],[TedToolkit.Results.Representation.Benchmarks.IntFailure, TedToolkit.Results.Representation.Benchmarks]].get_Value()
       push      rsi
       sub       rsp,20
       cmp       byte ptr [rcx+4],1
       jne       short M03_L00
       mov       eax,[rcx+8]
       add       rsp,20
       pop       rsi
       ret
M03_L00:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rsi,rax
       mov       rcx,rsi
       call      qword ptr [7FFE7EA43980]; System.InvalidOperationException..ctor()
       mov       rcx,rsi
       call      CORINFO_HELP_THROW
       int       3
; Total bytes of code 56
```

## .NET Framework 4.8.1 (4.8.9337.0), X64 RyuJIT VectorSize=256 (Job: DefaultJob)

```assembly
; TedToolkit.Results.Representation.Benchmarks.StateWidthBenchmarks.IntStateWithIntFailure()
       push      rdi
       push      rsi
       push      rbp
       push      rbx
       sub       rsp,28
       mov       rsi,rcx
       xor       eax,eax
       xor       edi,edi
       mov       rcx,[rsi+20]
       cmp       dword ptr [rcx+8],0
       jle       short M00_L03
M00_L00:
       mov       rcx,[rsi+20]
       cmp       edi,[rcx+8]
       jae       short M00_L04
       movsxd    rdx,edi
       lea       rdx,[rdx+rdx*2]
       lea       rbx,[rcx+rdx*4+10]
       mov       ebp,eax
       mov       rcx,rbx
       call      TedToolkit.Results.Representation.Benchmarks.IntStateResult`2[[System.Int32, mscorlib],[TedToolkit.Results.Representation.Benchmarks.IntFailure, TedToolkit.Results.Representation.Benchmarks]].get_IsSuccess()
       test      al,al
       jne       short M00_L01
       mov       rcx,rbx
       call      TedToolkit.Results.Representation.Benchmarks.IntStateResult`2[[System.Int32, mscorlib],[TedToolkit.Results.Representation.Benchmarks.IntFailure, TedToolkit.Results.Representation.Benchmarks]].get_FailureReason()
       jmp       short M00_L02
M00_L01:
       mov       rcx,rbx
       call      TedToolkit.Results.Representation.Benchmarks.IntStateResult`2[[System.Int32, mscorlib],[TedToolkit.Results.Representation.Benchmarks.IntFailure, TedToolkit.Results.Representation.Benchmarks]].get_Value()
M00_L02:
       add       eax,ebp
       inc       edi
       mov       rdx,[rsi+20]
       cmp       [rdx+8],edi
       jg        short M00_L00
M00_L03:
       add       rsp,28
       pop       rbx
       pop       rbp
       pop       rsi
       pop       rdi
       ret
M00_L04:
       call      CORINFO_HELP_RNGCHKFAIL
       int       3
; Total bytes of code 106
```
```assembly
; TedToolkit.Results.Representation.Benchmarks.IntStateResult`2[[System.Int32, mscorlib],[TedToolkit.Results.Representation.Benchmarks.IntFailure, TedToolkit.Results.Representation.Benchmarks]].get_IsSuccess()
       push      rsi
       sub       rsp,20
       mov       eax,[rcx+4]
       cmp       eax,1
       je        short M01_L01
       cmp       eax,2
       jne       short M01_L02
       xor       eax,eax
M01_L00:
       add       rsp,20
       pop       rsi
       ret
M01_L01:
       mov       eax,1
       jmp       short M01_L00
M01_L02:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rsi,rax
       mov       rcx,rsi
       call      qword ptr [7FFE7EA43980]; System.InvalidOperationException..ctor()
       mov       rcx,rsi
       call      CORINFO_HELP_THROW
       int       3
; Total bytes of code 69
```
```assembly
; TedToolkit.Results.Representation.Benchmarks.IntStateResult`2[[System.Int32, mscorlib],[TedToolkit.Results.Representation.Benchmarks.IntFailure, TedToolkit.Results.Representation.Benchmarks]].get_FailureReason()
       push      rsi
       sub       rsp,20
       cmp       dword ptr [rcx+4],2
       jne       short M02_L00
       mov       eax,[rcx]
       add       rsp,20
       pop       rsi
       ret
M02_L00:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rsi,rax
       mov       rcx,rsi
       call      qword ptr [7FFE7EA43980]; System.InvalidOperationException..ctor()
       mov       rcx,rsi
       call      CORINFO_HELP_THROW
       int       3
; Total bytes of code 55
```
```assembly
; TedToolkit.Results.Representation.Benchmarks.IntStateResult`2[[System.Int32, mscorlib],[TedToolkit.Results.Representation.Benchmarks.IntFailure, TedToolkit.Results.Representation.Benchmarks]].get_Value()
       push      rsi
       sub       rsp,20
       cmp       dword ptr [rcx+4],1
       jne       short M03_L00
       mov       eax,[rcx+8]
       add       rsp,20
       pop       rsi
       ret
M03_L00:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rsi,rax
       mov       rcx,rsi
       call      qword ptr [7FFE7EA43980]; System.InvalidOperationException..ctor()
       mov       rcx,rsi
       call      CORINFO_HELP_THROW
       int       3
; Total bytes of code 56
```

## .NET Framework 4.8.1 (4.8.9337.0), X64 RyuJIT VectorSize=256 (Job: DefaultJob)

```assembly
; TedToolkit.Results.Representation.Benchmarks.StateWidthBenchmarks.ByteStateWithByteFailure()
       push      r14
       push      rdi
       push      rsi
       push      rbp
       push      rbx
       sub       rsp,20
       mov       rsi,rcx
       xor       eax,eax
       xor       edi,edi
       mov       rcx,[rsi+8]
       cmp       dword ptr [rcx+8],0
       jle       short M00_L03
M00_L00:
       mov       rcx,[rsi+8]
       cmp       edi,[rcx+8]
       jae       short M00_L04
       movsxd    rdx,edi
       lea       rbx,[rcx+rdx*8+10]
       mov       ebp,eax
       mov       rcx,rbx
       call      TedToolkit.Results.Representation.Benchmarks.ByteStateResult`2[[System.Int32, mscorlib],[TedToolkit.Results.Representation.Benchmarks.ByteFailure, TedToolkit.Results.Representation.Benchmarks]].get_IsSuccess()
       test      al,al
       jne       short M00_L01
       mov       rcx,rbx
       call      TedToolkit.Results.Representation.Benchmarks.ByteStateResult`2[[System.Int32, mscorlib],[TedToolkit.Results.Representation.Benchmarks.ByteFailure, TedToolkit.Results.Representation.Benchmarks]].get_FailureReason()
       movzx     r14d,al
       jmp       short M00_L02
M00_L01:
       mov       rcx,rbx
       call      TedToolkit.Results.Representation.Benchmarks.ByteStateResult`2[[System.Int32, mscorlib],[TedToolkit.Results.Representation.Benchmarks.ByteFailure, TedToolkit.Results.Representation.Benchmarks]].get_Value()
       mov       r14d,eax
M00_L02:
       lea       eax,[r14+rbp]
       inc       edi
       mov       rdx,[rsi+8]
       cmp       [rdx+8],edi
       jg        short M00_L00
M00_L03:
       add       rsp,20
       pop       rbx
       pop       rbp
       pop       rsi
       pop       rdi
       pop       r14
       ret
M00_L04:
       call      CORINFO_HELP_RNGCHKFAIL
       int       3
; Total bytes of code 115
```
```assembly
; TedToolkit.Results.Representation.Benchmarks.ByteStateResult`2[[System.Int32, mscorlib],[TedToolkit.Results.Representation.Benchmarks.ByteFailure, TedToolkit.Results.Representation.Benchmarks]].get_IsSuccess()
       push      rsi
       sub       rsp,20
       movzx     eax,byte ptr [rcx+1]
       cmp       eax,1
       je        short M01_L01
       cmp       eax,2
       jne       short M01_L02
       xor       eax,eax
M01_L00:
       add       rsp,20
       pop       rsi
       ret
M01_L01:
       mov       eax,1
       jmp       short M01_L00
M01_L02:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rsi,rax
       mov       rcx,rsi
       call      qword ptr [7FFE7EA43980]; System.InvalidOperationException..ctor()
       mov       rcx,rsi
       call      CORINFO_HELP_THROW
       int       3
; Total bytes of code 70
```
```assembly
; TedToolkit.Results.Representation.Benchmarks.ByteStateResult`2[[System.Int32, mscorlib],[TedToolkit.Results.Representation.Benchmarks.ByteFailure, TedToolkit.Results.Representation.Benchmarks]].get_FailureReason()
       push      rsi
       sub       rsp,20
       cmp       byte ptr [rcx+1],2
       jne       short M02_L00
       movzx     eax,byte ptr [rcx]
       add       rsp,20
       pop       rsi
       ret
M02_L00:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rsi,rax
       mov       rcx,rsi
       call      qword ptr [7FFE7EA43980]; System.InvalidOperationException..ctor()
       mov       rcx,rsi
       call      CORINFO_HELP_THROW
       int       3
; Total bytes of code 56
```
```assembly
; TedToolkit.Results.Representation.Benchmarks.ByteStateResult`2[[System.Int32, mscorlib],[TedToolkit.Results.Representation.Benchmarks.ByteFailure, TedToolkit.Results.Representation.Benchmarks]].get_Value()
       push      rsi
       sub       rsp,20
       cmp       byte ptr [rcx+1],1
       jne       short M03_L00
       mov       eax,[rcx+4]
       add       rsp,20
       pop       rsi
       ret
M03_L00:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rsi,rax
       mov       rcx,rsi
       call      qword ptr [7FFE7EA43980]; System.InvalidOperationException..ctor()
       mov       rcx,rsi
       call      CORINFO_HELP_THROW
       int       3
; Total bytes of code 56
```

## .NET Framework 4.8.1 (4.8.9337.0), X64 RyuJIT VectorSize=256 (Job: DefaultJob)

```assembly
; TedToolkit.Results.Representation.Benchmarks.StateWidthBenchmarks.IntStateWithByteFailure()
       push      r14
       push      rdi
       push      rsi
       push      rbp
       push      rbx
       sub       rsp,20
       mov       rsi,rcx
       xor       eax,eax
       xor       edi,edi
       mov       rcx,[rsi+18]
       cmp       dword ptr [rcx+8],0
       jle       short M00_L03
M00_L00:
       mov       rcx,[rsi+18]
       cmp       edi,[rcx+8]
       jae       short M00_L04
       movsxd    rdx,edi
       lea       rdx,[rdx+rdx*2]
       lea       rbx,[rcx+rdx*4+10]
       mov       ebp,eax
       mov       rcx,rbx
       call      TedToolkit.Results.Representation.Benchmarks.IntStateResult`2[[System.Int32, mscorlib],[TedToolkit.Results.Representation.Benchmarks.ByteFailure, TedToolkit.Results.Representation.Benchmarks]].get_IsSuccess()
       test      al,al
       jne       short M00_L01
       mov       rcx,rbx
       call      TedToolkit.Results.Representation.Benchmarks.IntStateResult`2[[System.Int32, mscorlib],[TedToolkit.Results.Representation.Benchmarks.ByteFailure, TedToolkit.Results.Representation.Benchmarks]].get_FailureReason()
       movzx     r14d,al
       jmp       short M00_L02
M00_L01:
       mov       rcx,rbx
       call      TedToolkit.Results.Representation.Benchmarks.IntStateResult`2[[System.Int32, mscorlib],[TedToolkit.Results.Representation.Benchmarks.ByteFailure, TedToolkit.Results.Representation.Benchmarks]].get_Value()
       mov       r14d,eax
M00_L02:
       lea       eax,[r14+rbp]
       inc       edi
       mov       rdx,[rsi+18]
       cmp       [rdx+8],edi
       jg        short M00_L00
M00_L03:
       add       rsp,20
       pop       rbx
       pop       rbp
       pop       rsi
       pop       rdi
       pop       r14
       ret
M00_L04:
       call      CORINFO_HELP_RNGCHKFAIL
       int       3
; Total bytes of code 119
```
```assembly
; TedToolkit.Results.Representation.Benchmarks.IntStateResult`2[[System.Int32, mscorlib],[TedToolkit.Results.Representation.Benchmarks.ByteFailure, TedToolkit.Results.Representation.Benchmarks]].get_IsSuccess()
       push      rsi
       sub       rsp,20
       mov       eax,[rcx+4]
       cmp       eax,1
       je        short M01_L01
       cmp       eax,2
       jne       short M01_L02
       xor       eax,eax
M01_L00:
       add       rsp,20
       pop       rsi
       ret
M01_L01:
       mov       eax,1
       jmp       short M01_L00
M01_L02:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rsi,rax
       mov       rcx,rsi
       call      qword ptr [7FFE7EA43980]; System.InvalidOperationException..ctor()
       mov       rcx,rsi
       call      CORINFO_HELP_THROW
       int       3
; Total bytes of code 69
```
```assembly
; TedToolkit.Results.Representation.Benchmarks.IntStateResult`2[[System.Int32, mscorlib],[TedToolkit.Results.Representation.Benchmarks.ByteFailure, TedToolkit.Results.Representation.Benchmarks]].get_FailureReason()
       push      rsi
       sub       rsp,20
       cmp       dword ptr [rcx+4],2
       jne       short M02_L00
       movzx     eax,byte ptr [rcx]
       add       rsp,20
       pop       rsi
       ret
M02_L00:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rsi,rax
       mov       rcx,rsi
       call      qword ptr [7FFE7EA43980]; System.InvalidOperationException..ctor()
       mov       rcx,rsi
       call      CORINFO_HELP_THROW
       int       3
; Total bytes of code 56
```
```assembly
; TedToolkit.Results.Representation.Benchmarks.IntStateResult`2[[System.Int32, mscorlib],[TedToolkit.Results.Representation.Benchmarks.ByteFailure, TedToolkit.Results.Representation.Benchmarks]].get_Value()
       push      rsi
       sub       rsp,20
       cmp       dword ptr [rcx+4],1
       jne       short M03_L00
       mov       eax,[rcx+8]
       add       rsp,20
       pop       rsi
       ret
M03_L00:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rsi,rax
       mov       rcx,rsi
       call      qword ptr [7FFE7EA43980]; System.InvalidOperationException..ctor()
       mov       rcx,rsi
       call      CORINFO_HELP_THROW
       int       3
; Total bytes of code 56
```

## .NET Framework 4.8.1 (4.8.9337.0), X64 RyuJIT VectorSize=256 (Job: DefaultJob)

```assembly
; TedToolkit.Results.Representation.Benchmarks.StateWidthBenchmarks.ByteStateWithIntFailure()
       push      rdi
       push      rsi
       push      rbp
       push      rbx
       sub       rsp,28
       mov       rsi,rcx
       xor       eax,eax
       xor       edi,edi
       mov       rcx,[rsi+10]
       cmp       dword ptr [rcx+8],0
       jle       short M00_L03
M00_L00:
       mov       rcx,[rsi+10]
       cmp       edi,[rcx+8]
       jae       short M00_L04
       movsxd    rdx,edi
       lea       rdx,[rdx+rdx*2]
       lea       rbx,[rcx+rdx*4+10]
       mov       ebp,eax
       mov       rcx,rbx
       call      TedToolkit.Results.Representation.Benchmarks.ByteStateResult`2[[System.Int32, mscorlib],[TedToolkit.Results.Representation.Benchmarks.IntFailure, TedToolkit.Results.Representation.Benchmarks]].get_IsSuccess()
       test      al,al
       jne       short M00_L01
       mov       rcx,rbx
       call      TedToolkit.Results.Representation.Benchmarks.ByteStateResult`2[[System.Int32, mscorlib],[TedToolkit.Results.Representation.Benchmarks.IntFailure, TedToolkit.Results.Representation.Benchmarks]].get_FailureReason()
       jmp       short M00_L02
M00_L01:
       mov       rcx,rbx
       call      TedToolkit.Results.Representation.Benchmarks.ByteStateResult`2[[System.Int32, mscorlib],[TedToolkit.Results.Representation.Benchmarks.IntFailure, TedToolkit.Results.Representation.Benchmarks]].get_Value()
M00_L02:
       add       eax,ebp
       inc       edi
       mov       rdx,[rsi+10]
       cmp       [rdx+8],edi
       jg        short M00_L00
M00_L03:
       add       rsp,28
       pop       rbx
       pop       rbp
       pop       rsi
       pop       rdi
       ret
M00_L04:
       call      CORINFO_HELP_RNGCHKFAIL
       int       3
; Total bytes of code 106
```
```assembly
; TedToolkit.Results.Representation.Benchmarks.ByteStateResult`2[[System.Int32, mscorlib],[TedToolkit.Results.Representation.Benchmarks.IntFailure, TedToolkit.Results.Representation.Benchmarks]].get_IsSuccess()
       push      rsi
       sub       rsp,20
       movzx     eax,byte ptr [rcx+4]
       cmp       eax,1
       je        short M01_L01
       cmp       eax,2
       jne       short M01_L02
       xor       eax,eax
M01_L00:
       add       rsp,20
       pop       rsi
       ret
M01_L01:
       mov       eax,1
       jmp       short M01_L00
M01_L02:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rsi,rax
       mov       rcx,rsi
       call      qword ptr [7FFE7EA43980]; System.InvalidOperationException..ctor()
       mov       rcx,rsi
       call      CORINFO_HELP_THROW
       int       3
; Total bytes of code 70
```
```assembly
; TedToolkit.Results.Representation.Benchmarks.ByteStateResult`2[[System.Int32, mscorlib],[TedToolkit.Results.Representation.Benchmarks.IntFailure, TedToolkit.Results.Representation.Benchmarks]].get_FailureReason()
       push      rsi
       sub       rsp,20
       cmp       byte ptr [rcx+4],2
       jne       short M02_L00
       mov       eax,[rcx]
       add       rsp,20
       pop       rsi
       ret
M02_L00:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rsi,rax
       mov       rcx,rsi
       call      qword ptr [7FFE7EA43980]; System.InvalidOperationException..ctor()
       mov       rcx,rsi
       call      CORINFO_HELP_THROW
       int       3
; Total bytes of code 55
```
```assembly
; TedToolkit.Results.Representation.Benchmarks.ByteStateResult`2[[System.Int32, mscorlib],[TedToolkit.Results.Representation.Benchmarks.IntFailure, TedToolkit.Results.Representation.Benchmarks]].get_Value()
       push      rsi
       sub       rsp,20
       cmp       byte ptr [rcx+4],1
       jne       short M03_L00
       mov       eax,[rcx+8]
       add       rsp,20
       pop       rsi
       ret
M03_L00:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rsi,rax
       mov       rcx,rsi
       call      qword ptr [7FFE7EA43980]; System.InvalidOperationException..ctor()
       mov       rcx,rsi
       call      CORINFO_HELP_THROW
       int       3
; Total bytes of code 56
```

## .NET Framework 4.8.1 (4.8.9337.0), X64 RyuJIT VectorSize=256 (Job: DefaultJob)

```assembly
; TedToolkit.Results.Representation.Benchmarks.StateWidthBenchmarks.IntStateWithIntFailure()
       push      rdi
       push      rsi
       push      rbp
       push      rbx
       sub       rsp,28
       mov       rsi,rcx
       xor       eax,eax
       xor       edi,edi
       mov       rcx,[rsi+20]
       cmp       dword ptr [rcx+8],0
       jle       short M00_L03
M00_L00:
       mov       rcx,[rsi+20]
       cmp       edi,[rcx+8]
       jae       short M00_L04
       movsxd    rdx,edi
       lea       rdx,[rdx+rdx*2]
       lea       rbx,[rcx+rdx*4+10]
       mov       ebp,eax
       mov       rcx,rbx
       call      TedToolkit.Results.Representation.Benchmarks.IntStateResult`2[[System.Int32, mscorlib],[TedToolkit.Results.Representation.Benchmarks.IntFailure, TedToolkit.Results.Representation.Benchmarks]].get_IsSuccess()
       test      al,al
       jne       short M00_L01
       mov       rcx,rbx
       call      TedToolkit.Results.Representation.Benchmarks.IntStateResult`2[[System.Int32, mscorlib],[TedToolkit.Results.Representation.Benchmarks.IntFailure, TedToolkit.Results.Representation.Benchmarks]].get_FailureReason()
       jmp       short M00_L02
M00_L01:
       mov       rcx,rbx
       call      TedToolkit.Results.Representation.Benchmarks.IntStateResult`2[[System.Int32, mscorlib],[TedToolkit.Results.Representation.Benchmarks.IntFailure, TedToolkit.Results.Representation.Benchmarks]].get_Value()
M00_L02:
       add       eax,ebp
       inc       edi
       mov       rdx,[rsi+20]
       cmp       [rdx+8],edi
       jg        short M00_L00
M00_L03:
       add       rsp,28
       pop       rbx
       pop       rbp
       pop       rsi
       pop       rdi
       ret
M00_L04:
       call      CORINFO_HELP_RNGCHKFAIL
       int       3
; Total bytes of code 106
```
```assembly
; TedToolkit.Results.Representation.Benchmarks.IntStateResult`2[[System.Int32, mscorlib],[TedToolkit.Results.Representation.Benchmarks.IntFailure, TedToolkit.Results.Representation.Benchmarks]].get_IsSuccess()
       push      rsi
       sub       rsp,20
       mov       eax,[rcx+4]
       cmp       eax,1
       je        short M01_L01
       cmp       eax,2
       jne       short M01_L02
       xor       eax,eax
M01_L00:
       add       rsp,20
       pop       rsi
       ret
M01_L01:
       mov       eax,1
       jmp       short M01_L00
M01_L02:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rsi,rax
       mov       rcx,rsi
       call      qword ptr [7FFE7EA43980]; System.InvalidOperationException..ctor()
       mov       rcx,rsi
       call      CORINFO_HELP_THROW
       int       3
; Total bytes of code 69
```
```assembly
; TedToolkit.Results.Representation.Benchmarks.IntStateResult`2[[System.Int32, mscorlib],[TedToolkit.Results.Representation.Benchmarks.IntFailure, TedToolkit.Results.Representation.Benchmarks]].get_FailureReason()
       push      rsi
       sub       rsp,20
       cmp       dword ptr [rcx+4],2
       jne       short M02_L00
       mov       eax,[rcx]
       add       rsp,20
       pop       rsi
       ret
M02_L00:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rsi,rax
       mov       rcx,rsi
       call      qword ptr [7FFE7EA43980]; System.InvalidOperationException..ctor()
       mov       rcx,rsi
       call      CORINFO_HELP_THROW
       int       3
; Total bytes of code 55
```
```assembly
; TedToolkit.Results.Representation.Benchmarks.IntStateResult`2[[System.Int32, mscorlib],[TedToolkit.Results.Representation.Benchmarks.IntFailure, TedToolkit.Results.Representation.Benchmarks]].get_Value()
       push      rsi
       sub       rsp,20
       cmp       dword ptr [rcx+4],1
       jne       short M03_L00
       mov       eax,[rcx+8]
       add       rsp,20
       pop       rsi
       ret
M03_L00:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rsi,rax
       mov       rcx,rsi
       call      qword ptr [7FFE7EA43980]; System.InvalidOperationException..ctor()
       mov       rcx,rsi
       call      CORINFO_HELP_THROW
       int       3
; Total bytes of code 56
```

