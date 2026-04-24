# TestSamurai — 2D 사무라이 액션 프로젝트

Unity 2D 프로젝트입니다. 사무라이 캐릭터가 화면 왼쪽에서 달려와 중앙에 서고, 버튼을 눌러 공격 애니메이션과 슬래시 이펙트를 재생하는 것을 목표로 합니다.

---

## 목차
- [개요](#개요)
- [기능](#기능)
- [프로젝트 구조](#프로젝트-구조)
- [핵심 스크립트](#핵심-스크립트)
- [설계 패턴](#설계-패턴)
- [애니메이터 구조](#애니메이터-구조)
- [실행 방법](#실행-방법)

---

## 개요

| 항목 | 내용 |
|------|------|
| 엔진 | Unity (URP, 2D) |
| 언어 | C# |
| 렌더 파이프라인 | Universal Render Pipeline (URP) |
| 대상 플랫폼 | PC (Windows) |

---

## 기능

- 게임 시작 시 사무라이가 화면 왼쪽에서 중앙으로 달려옴
- **Attack** 버튼을 누르면 공격 애니메이션 재생
- 공격 시 칼 끝 위치에 슬래시 이펙트(`fx_samurai_slash`) 생성
- 이펙트는 재생 완료 후 자동으로 씬에서 제거
- `FxManager` 싱글톤이 씬 전환에도 유지되며 이펙트를 관리

---

## 프로젝트 구조

```
TestSamurai/
├── Assets/
│   ├── Animations/                     # 애니메이터 컨트롤러 및 클립
│   │   ├── Samurai/
│   │   │   ├── Samurai.controller      # 사무라이 Animator Controller (State: 0=Idle, 1=Attack, 2=Run)
│   │   │   ├── Samurai_Idle.anim
│   │   │   ├── Samurai_Attack.anim
│   │   │   ├── Samurai_Run.anim
│   │   │   └── Samurai_Slashes.anim
│   │   └── ...                         # 기타 애니메이션
│   ├── Prefabs/
│   │   ├── Samurai.prefab              # 사무라이 캐릭터 프리팹
│   │   └── fx_samurai_slash.prefab     # 슬래시 이펙트 프리팹
│   ├── Scenes/
│   │   ├── GameScene.unity             # 메인 게임 씬
│   │   └── SampleScene.unity
│   ├── Scripts/
│   │   ├── Samurai.cs                  # 사무라이 이동/공격 제어, 이벤트 발행
│   │   ├── GameMain.cs                 # 버튼 입력 처리, 이벤트 구독, FxManager 호출
│   │   └── FxManager.cs               # 이펙트 관리 싱글톤
│   ├── Free Pixel Art Forest/          # 배경 아트 에셋
│   ├── FREE_Samurai 2D Pixel Art v1.2/ # 사무라이 스프라이트 에셋
│   ├── Pixel Art Animations - Slashes/ # 슬래시 애니메이션 에셋
│   ├── Pixel UI pack 3/                # UI 에셋
│   ├── Sword Combat Sound Effects .../# 효과음 에셋
│   ├── TextMesh Pro/                   # TMP 에셋
│   └── Settings/                       # URP 렌더러 설정
├── Packages/                           # Unity 패키지 매니페스트
├── ProjectSettings/                    # 프로젝트 설정
├── .gitignore
└── README.md
```

---

## 핵심 스크립트

### `Samurai.cs`
사무라이 캐릭터의 이동과 공격을 담당합니다.

| 멤버 | 설명 |
|------|------|
| `event Action<Vector3> OnAttackStarted` | 공격 시작 시 칼 끝 위치를 전달하는 이벤트 |
| `Move(Vector3 tpos)` | 코루틴. 목표 위치까지 이동 후 Idle 전환 |
| `MoveToCenter()` | 중앙(Vector3.zero)으로 이동 시작 |
| `Attack()` | 공격 코루틴 시작. 진행 중인 이동 중단 |
| `slashSpawnPoint` | 슬래시 이펙트 생성 위치 (Inspector에서 자식 Transform 지정) |

### `GameMain.cs`
UI 버튼과 사무라이, FxManager를 연결하는 진입점입니다.

| 멤버 | 설명 |
|------|------|
| `attackButton` | Inspector에서 할당하는 Attack 버튼 |
| `samurai` | Inspector에서 할당하는 Samurai 참조 |
| `slashFxId` | 재생할 이펙트 ID (`"fx_samurai_slash"`) |
| `HandleSamuraiAttackStarted(Vector3)` | `OnAttackStarted` 이벤트 핸들러. FxManager 호출 |

### `FxManager.cs`
이펙트 생성과 수명 관리를 담당하는 싱글톤입니다.

| 멤버 | 설명 |
|------|------|
| `Instance` | 전역 싱글톤 인스턴스 |
| `fxEntries` | Inspector에서 id/prefab 쌍으로 등록하는 이펙트 목록 |
| `PlayFx(string id, Vector3 pos)` | 이펙트 생성. Inspector 미등록 시 `Resources.Load` 폴백 |
| `DestroyWhenFinished(GameObject)` | 코루틴. ParticleSystem/Animator 재생 완료 후 오브젝트 제거 |

---

## 설계 패턴

```
[Attack Button]
      │ onClick
      ▼
  GameMain
      │ samurai.Attack()
      ▼
   Samurai (코루틴: AttackRoutine)
      │ OnAttackStarted.Invoke(position)  ← 이벤트/대리자
      ▼
  GameMain.HandleSamuraiAttackStarted
      │ FxManager.Instance.PlayFx(...)
      ▼
  FxManager (싱글톤)
      │ Instantiate(prefab)
      │ StartCoroutine(DestroyWhenFinished)  ← 코루틴
      ▼
  fx_samurai_slash 생성 → 재생 완료 → Destroy
```

---

## 애니메이터 구조

`Samurai.controller` — `int` 파라미터 **State** 사용

| State 값 | 상태 |
|----------|------|
| 0 | Idle |
| 1 | Attack |
| 2 | Run |

**트랜지션 조건**

| From | To | 조건 |
|------|----|------|
| Idle | Attack | State == 1 |
| Idle | Run | State == 2 |
| Run | Attack | State == 1 |
| Run | Idle | State == 0 |
| Attack | Idle | State == 0 |

---

## 실행 방법

1. Unity Hub에서 프로젝트 열기
2. `GameScene` 씬 열기 (`Assets/Scenes/GameScene.unity`)
3. Hierarchy에서 `GameMain` 오브젝트 선택
   - `Attack Button` 필드에 UI Button 할당
   - `Samurai` 필드에 Samurai 프리팹 인스턴스 할당
4. Hierarchy에서 `FxManager` 오브젝트의 **Fx Entries**에 다음 항목 추가
   - `id`: `fx_samurai_slash`
   - `Prefab`: `Assets/Prefabs/fx_samurai_slash.prefab`
5. Play 버튼 실행 → Attack 버튼 클릭
