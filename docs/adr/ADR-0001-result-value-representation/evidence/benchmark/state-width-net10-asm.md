## .NET 10.0.10 (10.0.10, 10.0.1026.32716), X64 RyuJIT x86-64-v3 (Job: DefaultJob)

```assembly
; TedToolkit.Results.Representation.Benchmarks.StateWidthBenchmarks.ByteStateWithByteFailure()
       push      rbx
       sub       rsp,20
       xor       eax,eax
       xor       edx,edx
       mov       rcx,[rcx+8]
       cmp       dword ptr [rcx+8],0
       jg        short M00_L03
M00_L00:
       add       rsp,20
       pop       rbx
       ret
       nop       dword ptr [rax]
M00_L01:
       cmp       r9d,2
       jne       short M00_L04
       cmp       r10d,2
       jne       short M00_L05
       movzx     r9d,byte ptr [r8]
M00_L02:
       add       eax,r9d
       inc       edx
       cmp       [rcx+8],edx
       jle       short M00_L00
M00_L03:
       mov       r8,rcx
       cmp       edx,[r8+8]
       jae       near ptr M00_L07
       lea       r8,[r8+rdx*8+10]
       movzx     r10d,byte ptr [r8+1]
       mov       r9d,r10d
       cmp       r9d,1
       jne       short M00_L01
       cmp       r10d,1
       jne       short M00_L06
       mov       r9d,[r8+4]
       jmp       short M00_L02
M00_L04:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rbx,rax
       mov       rcx,rbx
       call      qword ptr [7FFD92C5FF78]
       mov       rcx,rbx
       call      CORINFO_HELP_THROW
       int       3
M00_L05:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rbx,rax
       mov       rcx,rbx
       call      qword ptr [7FFD92C5FF78]
       mov       rcx,rbx
       call      CORINFO_HELP_THROW
       int       3
M00_L06:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rbx,rax
       mov       rcx,rbx
       call      qword ptr [7FFD92C5FF78]
       mov       rcx,rbx
       call      CORINFO_HELP_THROW
       int       3
M00_L07:
       call      CORINFO_HELP_RNGCHKFAIL
       int       3
; Total bytes of code 216
```

## .NET 10.0.10 (10.0.10, 10.0.1026.32716), X64 RyuJIT x86-64-v3 (Job: DefaultJob)

```assembly
; TedToolkit.Results.Representation.Benchmarks.StateWidthBenchmarks.IntStateWithByteFailure()
       push      rbx
       sub       rsp,20
       xor       eax,eax
       xor       edx,edx
       mov       rcx,[rcx+18]
       cmp       dword ptr [rcx+8],0
       jg        short M00_L03
M00_L00:
       add       rsp,20
       pop       rbx
       ret
       nop       dword ptr [rax]
M00_L01:
       cmp       r10d,2
       jne       short M00_L04
       movzx     r9d,byte ptr [r8]
M00_L02:
       add       eax,r9d
       inc       edx
       cmp       [rcx+8],edx
       jle       short M00_L00
M00_L03:
       mov       r8,rcx
       cmp       edx,[r8+8]
       jae       short M00_L05
       lea       r10,[rdx+rdx*2]
       lea       r8,[r8+r10*4+10]
       mov       r10d,[r8+4]
       cmp       r10d,1
       jne       short M00_L01
       mov       r9d,[r8+8]
       jmp       short M00_L02
M00_L04:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rbx,rax
       mov       rcx,rbx
       call      qword ptr [7FFD92C7FEA0]
       mov       rcx,rbx
       call      CORINFO_HELP_THROW
       int       3
M00_L05:
       call      CORINFO_HELP_RNGCHKFAIL
       int       3
; Total bytes of code 128
```

## .NET 10.0.10 (10.0.10, 10.0.1026.32716), X64 RyuJIT x86-64-v3 (Job: DefaultJob)

```assembly
; TedToolkit.Results.Representation.Benchmarks.StateWidthBenchmarks.ByteStateWithIntFailure()
       push      rbx
       sub       rsp,20
       xor       eax,eax
       xor       edx,edx
       mov       rcx,[rcx+10]
       cmp       dword ptr [rcx+8],0
       jg        short M00_L03
M00_L00:
       add       rsp,20
       pop       rbx
       ret
       nop       dword ptr [rax]
M00_L01:
       cmp       r10d,1
       jne       near ptr M00_L06
       mov       r11d,[r8+8]
M00_L02:
       add       eax,r11d
       inc       edx
       cmp       [rcx+8],edx
       jle       short M00_L00
M00_L03:
       mov       r8,rcx
       cmp       edx,[r8+8]
       jae       near ptr M00_L07
       lea       r10,[rdx+rdx*2]
       lea       r8,[r8+r10*4+10]
       movzx     r10d,byte ptr [r8+4]
       mov       r9d,r10d
       cmp       r9d,1
       je        short M00_L01
       cmp       r9d,2
       jne       short M00_L04
       cmp       r10d,2
       jne       short M00_L05
       mov       r11d,[r8]
       jmp       short M00_L02
M00_L04:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rbx,rax
       mov       rcx,rbx
       call      qword ptr [7FFD92C6FEA0]
       mov       rcx,rbx
       call      CORINFO_HELP_THROW
       int       3
M00_L05:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rbx,rax
       mov       rcx,rbx
       call      qword ptr [7FFD92C6FEA0]
       mov       rcx,rbx
       call      CORINFO_HELP_THROW
       int       3
M00_L06:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rbx,rax
       mov       rcx,rbx
       call      qword ptr [7FFD92C6FEA0]
       mov       rcx,rbx
       call      CORINFO_HELP_THROW
       int       3
M00_L07:
       call      CORINFO_HELP_RNGCHKFAIL
       int       3
; Total bytes of code 223
```

## .NET 10.0.10 (10.0.10, 10.0.1026.32716), X64 RyuJIT x86-64-v3 (Job: DefaultJob)

```assembly
; TedToolkit.Results.Representation.Benchmarks.StateWidthBenchmarks.IntStateWithIntFailure()
       push      rbx
       sub       rsp,20
       xor       eax,eax
       xor       edx,edx
       mov       rcx,[rcx+20]
       cmp       dword ptr [rcx+8],0
       jg        short M00_L03
M00_L00:
       add       rsp,20
       pop       rbx
       ret
       nop       dword ptr [rax]
M00_L01:
       mov       r9d,[r8+8]
M00_L02:
       add       eax,r9d
       inc       edx
       cmp       [rcx+8],edx
       jle       short M00_L00
M00_L03:
       mov       r8,rcx
       cmp       edx,[r8+8]
       jae       short M00_L05
       lea       r10,[rdx+rdx*2]
       lea       r8,[r8+r10*4+10]
       mov       r10d,[r8+4]
       cmp       r10d,1
       je        short M00_L01
       cmp       r10d,2
       jne       short M00_L04
       mov       r9d,[r8]
       jmp       short M00_L02
M00_L04:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rbx,rax
       mov       rcx,rbx
       call      qword ptr [7FFD92C5FE88]
       mov       rcx,rbx
       call      CORINFO_HELP_THROW
       int       3
M00_L05:
       call      CORINFO_HELP_RNGCHKFAIL
       int       3
; Total bytes of code 127
```

## .NET 10.0.10 (10.0.10, 10.0.1026.32716), X64 RyuJIT x86-64-v3 (Job: DefaultJob)

```assembly
; TedToolkit.Results.Representation.Benchmarks.StateWidthBenchmarks.ByteStateWithByteFailure()
       push      rbx
       sub       rsp,20
       xor       eax,eax
       xor       edx,edx
       mov       rcx,[rcx+8]
       cmp       dword ptr [rcx+8],0
       jg        short M00_L03
M00_L00:
       add       rsp,20
       pop       rbx
       ret
       nop       dword ptr [rax]
M00_L01:
       cmp       r10d,1
       jne       near ptr M00_L06
       mov       r11d,[r8+4]
M00_L02:
       add       eax,r11d
       inc       edx
       cmp       [rcx+8],edx
       jle       short M00_L00
M00_L03:
       mov       r8,rcx
       cmp       edx,[r8+8]
       jae       near ptr M00_L07
       lea       r8,[r8+rdx*8+10]
       movzx     r10d,byte ptr [r8+1]
       mov       r9d,r10d
       cmp       r9d,1
       je        short M00_L01
       cmp       r9d,2
       jne       short M00_L04
       cmp       r10d,2
       jne       short M00_L05
       movzx     r11d,byte ptr [r8]
       jmp       short M00_L02
M00_L04:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rbx,rax
       mov       rcx,rbx
       call      qword ptr [7FFD92C7FEA0]
       mov       rcx,rbx
       call      CORINFO_HELP_THROW
       int       3
M00_L05:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rbx,rax
       mov       rcx,rbx
       call      qword ptr [7FFD92C7FEA0]
       mov       rcx,rbx
       call      CORINFO_HELP_THROW
       int       3
M00_L06:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rbx,rax
       mov       rcx,rbx
       call      qword ptr [7FFD92C7FEA0]
       mov       rcx,rbx
       call      CORINFO_HELP_THROW
       int       3
M00_L07:
       call      CORINFO_HELP_RNGCHKFAIL
       int       3
; Total bytes of code 220
```

## .NET 10.0.10 (10.0.10, 10.0.1026.32716), X64 RyuJIT x86-64-v3 (Job: DefaultJob)

```assembly
; TedToolkit.Results.Representation.Benchmarks.StateWidthBenchmarks.IntStateWithByteFailure()
       push      rbx
       sub       rsp,20
       xor       eax,eax
       xor       edx,edx
       mov       rcx,[rcx+18]
       cmp       dword ptr [rcx+8],0
       jg        short M00_L03
M00_L00:
       add       rsp,20
       pop       rbx
       ret
       nop       dword ptr [rax]
M00_L01:
       mov       r9d,[r8+8]
M00_L02:
       add       eax,r9d
       inc       edx
       cmp       [rcx+8],edx
       jle       short M00_L00
M00_L03:
       mov       r8,rcx
       cmp       edx,[r8+8]
       jae       short M00_L05
       lea       r10,[rdx+rdx*2]
       lea       r8,[r8+r10*4+10]
       mov       r10d,[r8+4]
       cmp       r10d,1
       je        short M00_L01
       cmp       r10d,2
       jne       short M00_L04
       movzx     r9d,byte ptr [r8]
       jmp       short M00_L02
M00_L04:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rbx,rax
       mov       rcx,rbx
       call      qword ptr [7FFD92C7FEA0]
       mov       rcx,rbx
       call      CORINFO_HELP_THROW
       int       3
M00_L05:
       call      CORINFO_HELP_RNGCHKFAIL
       int       3
; Total bytes of code 128
```

## .NET 10.0.10 (10.0.10, 10.0.1026.32716), X64 RyuJIT x86-64-v3 (Job: DefaultJob)

```assembly
; TedToolkit.Results.Representation.Benchmarks.StateWidthBenchmarks.ByteStateWithIntFailure()
       push      rbx
       sub       rsp,20
       xor       eax,eax
       xor       edx,edx
       mov       rcx,[rcx+10]
       cmp       dword ptr [rcx+8],0
       jg        short M00_L03
M00_L00:
       add       rsp,20
       pop       rbx
       ret
       nop       dword ptr [rax]
M00_L01:
       cmp       r10d,1
       jne       near ptr M00_L06
       mov       r11d,[r8+8]
M00_L02:
       add       eax,r11d
       inc       edx
       cmp       [rcx+8],edx
       jle       short M00_L00
M00_L03:
       mov       r8,rcx
       cmp       edx,[r8+8]
       jae       near ptr M00_L07
       lea       r10,[rdx+rdx*2]
       lea       r8,[r8+r10*4+10]
       movzx     r10d,byte ptr [r8+4]
       mov       r9d,r10d
       cmp       r9d,1
       je        short M00_L01
       cmp       r9d,2
       jne       short M00_L04
       cmp       r10d,2
       jne       short M00_L05
       mov       r11d,[r8]
       jmp       short M00_L02
M00_L04:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rbx,rax
       mov       rcx,rbx
       call      qword ptr [7FFD92C8FF90]
       mov       rcx,rbx
       call      CORINFO_HELP_THROW
       int       3
M00_L05:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rbx,rax
       mov       rcx,rbx
       call      qword ptr [7FFD92C8FF90]
       mov       rcx,rbx
       call      CORINFO_HELP_THROW
       int       3
M00_L06:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rbx,rax
       mov       rcx,rbx
       call      qword ptr [7FFD92C8FF90]
       mov       rcx,rbx
       call      CORINFO_HELP_THROW
       int       3
M00_L07:
       call      CORINFO_HELP_RNGCHKFAIL
       int       3
; Total bytes of code 223
```

## .NET 10.0.10 (10.0.10, 10.0.1026.32716), X64 RyuJIT x86-64-v3 (Job: DefaultJob)

```assembly
; TedToolkit.Results.Representation.Benchmarks.StateWidthBenchmarks.IntStateWithIntFailure()
       push      rbx
       sub       rsp,20
       xor       eax,eax
       xor       edx,edx
       mov       rcx,[rcx+20]
       cmp       dword ptr [rcx+8],0
       jg        short M00_L03
M00_L00:
       add       rsp,20
       pop       rbx
       ret
       nop       dword ptr [rax]
M00_L01:
       mov       r9d,[r8+8]
M00_L02:
       add       eax,r9d
       inc       edx
       cmp       [rcx+8],edx
       jle       short M00_L00
M00_L03:
       mov       r8,rcx
       cmp       edx,[r8+8]
       jae       short M00_L05
       lea       r10,[rdx+rdx*2]
       lea       r8,[r8+r10*4+10]
       mov       r10d,[r8+4]
       cmp       r10d,1
       je        short M00_L01
       cmp       r10d,2
       jne       short M00_L04
       mov       r9d,[r8]
       jmp       short M00_L02
M00_L04:
       mov       rcx,offset MT_System.InvalidOperationException
       call      CORINFO_HELP_NEWSFAST
       mov       rbx,rax
       mov       rcx,rbx
       call      qword ptr [7FFD92C6FF90]
       mov       rcx,rbx
       call      CORINFO_HELP_THROW
       int       3
M00_L05:
       call      CORINFO_HELP_RNGCHKFAIL
       int       3
; Total bytes of code 127
```

