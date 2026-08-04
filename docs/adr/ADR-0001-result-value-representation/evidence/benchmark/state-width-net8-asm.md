## .NET 8.0.29 (8.0.29, 8.0.2926.32403), X64 RyuJIT x86-64-v3 (Job: DefaultJob)

```assembly
; TedToolkit.Results.Representation.Benchmarks.StateWidthBenchmarks.ByteStateWithByteFailure()
       push      rbx
       sub       rsp,20
       xor       eax,eax
       xor       edx,edx
       mov       rcx,[rcx+8]
       cmp       dword ptr [rcx+8],0
       jle       short M00_L05
M00_L00:
       mov       r8,rcx
       cmp       edx,[r8+8]
       jae       near ptr M00_L09
       mov       r10d,edx
       lea       r8,[r8+r10*8+10]
       movzx     r10d,byte ptr [r8+1]
       mov       r9d,r10d
       cmp       r9d,1
       je        short M00_L04
       cmp       r9d,2
       jne       short M00_L06
       xor       r9d,r9d
M00_L01:
       test      r9d,r9d
       je        short M00_L03
       cmp       r10d,1
       jne       short M00_L08
       mov       r9d,[r8+4]
M00_L02:
       add       eax,r9d
       inc       edx
       cmp       [rcx+8],edx
       jg        short M00_L00
       jmp       short M00_L05
M00_L03:
       cmp       r10d,2
       jne       short M00_L07
       movzx     r9d,byte ptr [r8]
       jmp       short M00_L02
M00_L04:
       mov       r9d,1
       jmp       short M00_L01
M00_L05:
       add       rsp,20
       pop       rbx
       ret
M00_L06:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rbx,rax
       mov       rcx,rbx
       call      qword ptr [7FFE207A6700]
       mov       rcx,rbx
       call      CORINFO_HELP_THROW
M00_L07:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rbx,rax
       mov       rcx,rbx
       call      qword ptr [7FFE207A6700]
       mov       rcx,rbx
       call      CORINFO_HELP_THROW
M00_L08:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rbx,rax
       mov       rcx,rbx
       call      qword ptr [7FFE207A6700]
       mov       rcx,rbx
       call      CORINFO_HELP_THROW
       int       3
M00_L09:
       call      CORINFO_HELP_RNGCHKFAIL
       int       3
; Total bytes of code 228
```

## .NET 8.0.29 (8.0.29, 8.0.2926.32403), X64 RyuJIT x86-64-v3 (Job: DefaultJob)

```assembly
; TedToolkit.Results.Representation.Benchmarks.StateWidthBenchmarks.IntStateWithByteFailure()
       push      rbx
       sub       rsp,20
       xor       eax,eax
       xor       edx,edx
       mov       rcx,[rcx+18]
       cmp       dword ptr [rcx+8],0
       jle       short M00_L05
M00_L00:
       mov       r8,rcx
       cmp       edx,[r8+8]
       jae       near ptr M00_L09
       mov       r10d,edx
       lea       r10,[r10+r10*2]
       lea       r8,[r8+r10*4+10]
       mov       r10d,[r8+4]
       mov       r9d,r10d
       cmp       r9d,1
       je        short M00_L04
       cmp       r9d,2
       jne       short M00_L06
       xor       r9d,r9d
M00_L01:
       test      r9d,r9d
       je        short M00_L03
       cmp       r10d,1
       jne       short M00_L08
       mov       r9d,[r8+8]
M00_L02:
       add       eax,r9d
       inc       edx
       cmp       [rcx+8],edx
       jg        short M00_L00
       jmp       short M00_L05
M00_L03:
       cmp       r10d,2
       jne       short M00_L07
       movzx     r9d,byte ptr [r8]
       jmp       short M00_L02
M00_L04:
       mov       r9d,1
       jmp       short M00_L01
M00_L05:
       add       rsp,20
       pop       rbx
       ret
M00_L06:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rbx,rax
       mov       rcx,rbx
       call      qword ptr [7FFE20786700]
       mov       rcx,rbx
       call      CORINFO_HELP_THROW
M00_L07:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rbx,rax
       mov       rcx,rbx
       call      qword ptr [7FFE20786700]
       mov       rcx,rbx
       call      CORINFO_HELP_THROW
M00_L08:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rbx,rax
       mov       rcx,rbx
       call      qword ptr [7FFE20786700]
       mov       rcx,rbx
       call      CORINFO_HELP_THROW
       int       3
M00_L09:
       call      CORINFO_HELP_RNGCHKFAIL
       int       3
; Total bytes of code 231
```

## .NET 8.0.29 (8.0.29, 8.0.2926.32403), X64 RyuJIT x86-64-v3 (Job: DefaultJob)

```assembly
; TedToolkit.Results.Representation.Benchmarks.StateWidthBenchmarks.ByteStateWithIntFailure()
       push      rbx
       sub       rsp,20
       xor       eax,eax
       xor       edx,edx
       mov       rcx,[rcx+10]
       cmp       dword ptr [rcx+8],0
       jle       short M00_L05
M00_L00:
       mov       r8,rcx
       cmp       edx,[r8+8]
       jae       near ptr M00_L09
       mov       r10d,edx
       lea       r10,[r10+r10*2]
       lea       r8,[r8+r10*4+10]
       movzx     r10d,byte ptr [r8+4]
       mov       r9d,r10d
       cmp       r9d,1
       je        short M00_L04
       cmp       r9d,2
       jne       short M00_L06
       xor       r9d,r9d
M00_L01:
       test      r9d,r9d
       je        short M00_L03
       cmp       r10d,1
       jne       short M00_L08
       mov       r9d,[r8+8]
M00_L02:
       add       eax,r9d
       inc       edx
       cmp       [rcx+8],edx
       jg        short M00_L00
       jmp       short M00_L05
M00_L03:
       cmp       r10d,2
       jne       short M00_L07
       mov       r9d,[r8]
       jmp       short M00_L02
M00_L04:
       mov       r9d,1
       jmp       short M00_L01
M00_L05:
       add       rsp,20
       pop       rbx
       ret
M00_L06:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rbx,rax
       mov       rcx,rbx
       call      qword ptr [7FFE20776700]
       mov       rcx,rbx
       call      CORINFO_HELP_THROW
M00_L07:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rbx,rax
       mov       rcx,rbx
       call      qword ptr [7FFE20776700]
       mov       rcx,rbx
       call      CORINFO_HELP_THROW
M00_L08:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rbx,rax
       mov       rcx,rbx
       call      qword ptr [7FFE20776700]
       mov       rcx,rbx
       call      CORINFO_HELP_THROW
       int       3
M00_L09:
       call      CORINFO_HELP_RNGCHKFAIL
       int       3
; Total bytes of code 231
```

## .NET 8.0.29 (8.0.29, 8.0.2926.32403), X64 RyuJIT x86-64-v3 (Job: DefaultJob)

```assembly
; TedToolkit.Results.Representation.Benchmarks.StateWidthBenchmarks.IntStateWithIntFailure()
       push      rbx
       sub       rsp,20
       xor       eax,eax
       xor       edx,edx
       mov       rcx,[rcx+20]
       cmp       dword ptr [rcx+8],0
       jle       short M00_L05
M00_L00:
       mov       r8,rcx
       cmp       edx,[r8+8]
       jae       near ptr M00_L09
       mov       r10d,edx
       lea       r10,[r10+r10*2]
       lea       r8,[r8+r10*4+10]
       mov       r10d,[r8+4]
       mov       r9d,r10d
       cmp       r9d,1
       je        short M00_L04
       cmp       r9d,2
       jne       short M00_L06
       xor       r9d,r9d
M00_L01:
       test      r9d,r9d
       je        short M00_L03
       cmp       r10d,1
       jne       short M00_L08
       mov       r9d,[r8+8]
M00_L02:
       add       eax,r9d
       inc       edx
       cmp       [rcx+8],edx
       jg        short M00_L00
       jmp       short M00_L05
M00_L03:
       cmp       r10d,2
       jne       short M00_L07
       mov       r9d,[r8]
       jmp       short M00_L02
M00_L04:
       mov       r9d,1
       jmp       short M00_L01
M00_L05:
       add       rsp,20
       pop       rbx
       ret
M00_L06:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rbx,rax
       mov       rcx,rbx
       call      qword ptr [7FFE20776700]
       mov       rcx,rbx
       call      CORINFO_HELP_THROW
M00_L07:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rbx,rax
       mov       rcx,rbx
       call      qword ptr [7FFE20776700]
       mov       rcx,rbx
       call      CORINFO_HELP_THROW
M00_L08:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rbx,rax
       mov       rcx,rbx
       call      qword ptr [7FFE20776700]
       mov       rcx,rbx
       call      CORINFO_HELP_THROW
       int       3
M00_L09:
       call      CORINFO_HELP_RNGCHKFAIL
       int       3
; Total bytes of code 230
```

## .NET 8.0.29 (8.0.29, 8.0.2926.32403), X64 RyuJIT x86-64-v3 (Job: DefaultJob)

```assembly
; TedToolkit.Results.Representation.Benchmarks.StateWidthBenchmarks.ByteStateWithByteFailure()
       push      rbx
       sub       rsp,20
       xor       eax,eax
       xor       edx,edx
       mov       rcx,[rcx+8]
       cmp       dword ptr [rcx+8],0
       jle       short M00_L03
M00_L00:
       mov       r8,rcx
       cmp       edx,[r8+8]
       jae       near ptr M00_L09
       mov       r10d,edx
       lea       r8,[r8+r10*8+10]
       movzx     r10d,byte ptr [r8+1]
       mov       r9d,r10d
       cmp       r9d,1
       je        short M00_L05
       cmp       r9d,2
       jne       short M00_L06
       xor       r9d,r9d
M00_L01:
       test      r9d,r9d
       jne       short M00_L04
       cmp       r10d,2
       jne       short M00_L07
       movzx     r9d,byte ptr [r8]
M00_L02:
       add       eax,r9d
       inc       edx
       cmp       [rcx+8],edx
       jg        short M00_L00
M00_L03:
       add       rsp,20
       pop       rbx
       ret
M00_L04:
       cmp       r10d,1
       jne       short M00_L08
       mov       r9d,[r8+4]
       jmp       short M00_L02
M00_L05:
       mov       r9d,1
       jmp       short M00_L01
M00_L06:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rbx,rax
       mov       rcx,rbx
       call      qword ptr [7FFE207A6700]
       mov       rcx,rbx
       call      CORINFO_HELP_THROW
M00_L07:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rbx,rax
       mov       rcx,rbx
       call      qword ptr [7FFE207A6700]
       mov       rcx,rbx
       call      CORINFO_HELP_THROW
M00_L08:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rbx,rax
       mov       rcx,rbx
       call      qword ptr [7FFE207A6700]
       mov       rcx,rbx
       call      CORINFO_HELP_THROW
       int       3
M00_L09:
       call      CORINFO_HELP_RNGCHKFAIL
       int       3
; Total bytes of code 226
```

## .NET 8.0.29 (8.0.29, 8.0.2926.32403), X64 RyuJIT x86-64-v3 (Job: DefaultJob)

```assembly
; TedToolkit.Results.Representation.Benchmarks.StateWidthBenchmarks.IntStateWithByteFailure()
       push      rbx
       sub       rsp,20
       xor       eax,eax
       xor       edx,edx
       mov       rcx,[rcx+18]
       cmp       dword ptr [rcx+8],0
       jle       short M00_L03
M00_L00:
       mov       r8,rcx
       cmp       edx,[r8+8]
       jae       near ptr M00_L09
       mov       r10d,edx
       lea       r10,[r10+r10*2]
       lea       r8,[r8+r10*4+10]
       mov       r10d,[r8+4]
       mov       r9d,r10d
       cmp       r9d,1
       je        short M00_L05
       cmp       r9d,2
       jne       short M00_L06
       xor       r9d,r9d
M00_L01:
       test      r9d,r9d
       jne       short M00_L04
       cmp       r10d,2
       jne       short M00_L07
       movzx     r9d,byte ptr [r8]
M00_L02:
       add       eax,r9d
       inc       edx
       cmp       [rcx+8],edx
       jg        short M00_L00
M00_L03:
       add       rsp,20
       pop       rbx
       ret
M00_L04:
       cmp       r10d,1
       jne       short M00_L08
       mov       r9d,[r8+8]
       jmp       short M00_L02
M00_L05:
       mov       r9d,1
       jmp       short M00_L01
M00_L06:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rbx,rax
       mov       rcx,rbx
       call      qword ptr [7FFE20796700]
       mov       rcx,rbx
       call      CORINFO_HELP_THROW
M00_L07:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rbx,rax
       mov       rcx,rbx
       call      qword ptr [7FFE20796700]
       mov       rcx,rbx
       call      CORINFO_HELP_THROW
M00_L08:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rbx,rax
       mov       rcx,rbx
       call      qword ptr [7FFE20796700]
       mov       rcx,rbx
       call      CORINFO_HELP_THROW
       int       3
M00_L09:
       call      CORINFO_HELP_RNGCHKFAIL
       int       3
; Total bytes of code 229
```

## .NET 8.0.29 (8.0.29, 8.0.2926.32403), X64 RyuJIT x86-64-v3 (Job: DefaultJob)

```assembly
; TedToolkit.Results.Representation.Benchmarks.StateWidthBenchmarks.ByteStateWithIntFailure()
       push      rbx
       sub       rsp,20
       xor       eax,eax
       xor       edx,edx
       mov       rcx,[rcx+10]
       cmp       dword ptr [rcx+8],0
       jle       short M00_L03
M00_L00:
       mov       r8,rcx
       cmp       edx,[r8+8]
       jae       near ptr M00_L09
       mov       r10d,edx
       lea       r10,[r10+r10*2]
       lea       r8,[r8+r10*4+10]
       movzx     r10d,byte ptr [r8+4]
       mov       r9d,r10d
       cmp       r9d,1
       je        short M00_L05
       cmp       r9d,2
       jne       short M00_L06
       xor       r9d,r9d
M00_L01:
       test      r9d,r9d
       jne       short M00_L04
       cmp       r10d,2
       jne       short M00_L07
       mov       r9d,[r8]
M00_L02:
       add       eax,r9d
       inc       edx
       cmp       [rcx+8],edx
       jg        short M00_L00
M00_L03:
       add       rsp,20
       pop       rbx
       ret
M00_L04:
       cmp       r10d,1
       jne       short M00_L08
       mov       r9d,[r8+8]
       jmp       short M00_L02
M00_L05:
       mov       r9d,1
       jmp       short M00_L01
M00_L06:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rbx,rax
       mov       rcx,rbx
       call      qword ptr [7FFE20786700]
       mov       rcx,rbx
       call      CORINFO_HELP_THROW
M00_L07:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rbx,rax
       mov       rcx,rbx
       call      qword ptr [7FFE20786700]
       mov       rcx,rbx
       call      CORINFO_HELP_THROW
M00_L08:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rbx,rax
       mov       rcx,rbx
       call      qword ptr [7FFE20786700]
       mov       rcx,rbx
       call      CORINFO_HELP_THROW
       int       3
M00_L09:
       call      CORINFO_HELP_RNGCHKFAIL
       int       3
; Total bytes of code 229
```

## .NET 8.0.29 (8.0.29, 8.0.2926.32403), X64 RyuJIT x86-64-v3 (Job: DefaultJob)

```assembly
; TedToolkit.Results.Representation.Benchmarks.StateWidthBenchmarks.IntStateWithIntFailure()
       push      rbx
       sub       rsp,20
       xor       eax,eax
       xor       edx,edx
       mov       rcx,[rcx+20]
       cmp       dword ptr [rcx+8],0
       jle       short M00_L03
M00_L00:
       mov       r8,rcx
       cmp       edx,[r8+8]
       jae       near ptr M00_L09
       mov       r10d,edx
       lea       r10,[r10+r10*2]
       lea       r8,[r8+r10*4+10]
       mov       r10d,[r8+4]
       mov       r9d,r10d
       cmp       r9d,1
       je        short M00_L05
       cmp       r9d,2
       jne       short M00_L06
       xor       r9d,r9d
M00_L01:
       test      r9d,r9d
       jne       short M00_L04
       cmp       r10d,2
       jne       short M00_L07
       mov       r9d,[r8]
M00_L02:
       add       eax,r9d
       inc       edx
       cmp       [rcx+8],edx
       jg        short M00_L00
M00_L03:
       add       rsp,20
       pop       rbx
       ret
M00_L04:
       cmp       r10d,1
       jne       short M00_L08
       mov       r9d,[r8+8]
       jmp       short M00_L02
M00_L05:
       mov       r9d,1
       jmp       short M00_L01
M00_L06:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rbx,rax
       mov       rcx,rbx
       call      qword ptr [7FFE20796700]
       mov       rcx,rbx
       call      CORINFO_HELP_THROW
M00_L07:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rbx,rax
       mov       rcx,rbx
       call      qword ptr [7FFE20796700]
       mov       rcx,rbx
       call      CORINFO_HELP_THROW
M00_L08:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rbx,rax
       mov       rcx,rbx
       call      qword ptr [7FFE20796700]
       mov       rcx,rbx
       call      CORINFO_HELP_THROW
       int       3
M00_L09:
       call      CORINFO_HELP_RNGCHKFAIL
       int       3
; Total bytes of code 228
```

