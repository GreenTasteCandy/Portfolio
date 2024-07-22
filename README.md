# 🖥️ GTC_Portfolio
+ 연락처 : rugx@naver.com
+ 유니티 팀 프로젝트에서 참여하여 작업한 코드들을 모아놓은 Git입니다
---
## 📑 목차
[1. 🪶 Eggururu Classic](https://github.com/GreenTasteCandy/Portfolio?tab=readme-ov-file#1--eggururu-classic)

[2. 🌳 Folast](https://github.com/GreenTasteCandy/Portfolio?tab=readme-ov-file#2--folast)

[3. 🪐 Dimension Squad](https://github.com/GreenTasteCandy/Portfolio?tab=readme-ov-file#3--dimension-squad)

[4. 🚀 Astra](https://github.com/GreenTasteCandy/Portfolio?tab=readme-ov-file#4--astra)

[5. 🥚 Eggururu](https://github.com/GreenTasteCandy/Portfolio?tab=readme-ov-file#5--eggururu)

---
## 1. 🪶 Eggururu Classic

+ 2022년 8월 출시된 에구루루(2022)에서 사용된 코드들입니다
+ 장르 : 캐주얼 러닝
+ 다운로드(Google Play) : https://play.google.com/store/apps/details?id=com.TeamDUJ.Eggururu&hl=ko
+ 플레이 영상 : https://www.youtube.com/watch?v=RXmGPkEsk0E
+ 담당 업무 : 전반적인 게임의 모든 시스템 제작

### 💾 제작한 코드 설명
+ BreakMove.cs : 맵에서 길을 생성하는 오브젝트의 이동 코드입니다
+ CameraMove.cs : 카메라의 이동 코드입니다
+ CloudSave.cs : PGS(Play Games Service)에서 제공되는 기능을 통해 클라우드 저장을 사용하는 코드입니다
+ EggShop.cs : 메인로비의 스킨상점 코드입니다
+ GameOverScreen.cs : 인게임에서 게임오버 시 나오는 화면을 표시하는 코드입니다
+ GameSystem.cs : 인게임에서 플레이 타임,오브젝트의 생성,화면 UI 표시를 담당하는 코드입니다
+ GPGSBinder.cs : PGS에서 제공하는 기능들을 유니티에서 사용 가능하도록 제작한 코드입니다
+ LoadingSceneManager.cs : 로딩 화면을 구현한 코드입니다
+ meshEgg.cs : 플레이어 캐릭터의 스킨을 적용하는 코드입니다
+ Move.cs : 플레이어 캐릭터의 이동 및 스킬을 담당하는 코드입니다
+ MoveObj.cs : 장애물과 장식 오브젝트의 이동을 담당하는 코드입니다 
+ RewardBirthbutton.cs : Admob의 보상형광고 기능을 유니티에서 사용 가능하도록 제작한 코드입니다
+ rotateEgg.cs : 메인화면에서 회전하고있는 캐릭터 마네킹을 담당하는 코드입니다
+ shieldEffectMove.cs : 보호막 스킬 작동 시 생성되는 보호막 오브젝트의 이동 기능을 담당하는 코드입니다
+ SkinButtonCreate.cs : 스킨 상점 접속 시 등장하는 스킨 구매 버튼들을 표시하는 코드입니다
+ SkinButtonSet.cs : 스킨 버튼의 표시,선택 기능을 담당하는 코드입니다
+ StartGame.cs : 게임을 시작할 때 최초로 게임 내에서 필요한 변수를 선언하고 실행시켜야 할 기능들을 실행시키는 코드입니다
---
## 2. 🌳 Folast

+ 2022년 12월 출시된 포라스트에서 사용된 코드들입니다
+ 장르 : 디펜스 + 오펜스
+ 다운로드(Google Play) : https://play.google.com/store/apps/details?id=duj.teamDuj.Folast&hl=ko
+ 플레이 영상 : https://www.youtube.com/watch?v=oztcVhuSyEs
+ 담당 업무 : PGS를 통한 저장 및 업적과 랭킹 시스템 구현,디펜스 시스템과 오펜스 시스템,아이템 시스템,오브젝트의 이동 및 전투 시스템,타워 생성 시스템

### 💾 제작한 코드 설명
+ BuildTowerImage.cs : 인게임에서 유닛이나 영웅 버튼의 ui 표시 및 기능을 담당하는 코드입니다
+ CloudSave.cs : PGS(Play Games Service)에서 제공되는 기능을 통해 클라우드 저장을 사용하는 코드입니다
+ DefenceTemplate.cs : 스크립터블오브젝트를 통해 디펜스 시 등장하는 적 오브젝트의 생성 패턴을 설정하는 코드입니다
+ Enemy.cs : 오브젝트를 경로에 맞게 이동시키는 코드입니다
+ EnemyHP.cs : 오브젝트의 체력과 체력의 증감 기능을 담당하는 코드입니다
+ EnemyHPViewer.cs : 오브젝트의 체력 표시를 담당하는 코드입니다
+ EnemySpawn.cs : 적 오브젝트를 레벨디자인에 맞게 생성시키고 라운드를 진행시키는 코드입니다
+ FadeScreen.cs : 게임 오버 시 실행되는 페이드인/페이드아웃 연출을 담당하는 코드입니다
+ GameEndView.cs : 게임 종료 시 등장하는 화면의 UI와 실행되는 업적 해금/랭킹 등록 기능을 담당하는 코드입니다
+ GameManager.cs : 인게임에서 사용되는 코드들을 관리하는 싱글톤 패턴의 코드입니다
+ GameSpeed.cs : 게임의 배속 기능을 담당하는 코드입니다
+ InvenSlotView.cs : 아이템 선택지 화면에서 현재 보유중인 아이템을 확인할 수 있는 인벤토리를 표시하는 코드입니다
+ InvenStatusText.cs : 현재 확인중인 아이템의 능력치를 표시하는 코드입니다
+ InventorySet.cs : 인벤토리 화면에서 아이템 능력치를 슬롯별로 받아 와 사용하는 코드입니다
+ InventoryView.cs : 인벤토리 화면 UI를 표시하는 코드입니다
+ ItemSelectViewer.cs : 아이템 선택 화면을 표시하는 코드입니다
+ ItemSpawn.cs : 아이템 랜덤 생성,아이템 재배열,아이템 선택,인벤토리창 on/off와 같은 아이템 및 인벤토리 기능을 담당하는 코드입니다
+ ItemTemplate.cs : 스크립터블오브젝트를 통해 아이템의 능력치를 설정하는 코드입니다
+ LoadingSceneManager.cs : 로딩 화면을 구현한 코드입니다
+ Lobby.cs : 로비 씬에서 각 버튼들 기능,재화 UI 출력을 담당하는 코드입니다
+ MapTemplate.cs : 스크립터블오브젝트를 통해 필드의 맵 구조를 설정하는 코드입니다
+ Movement2D.cs : 2D 좌표평면에서 오브젝트를 이동시키는 코드입니다
+ ObjectDetector.cs : 인게임에서 타일 터치 기능을 구현한 코드입니다
+ OffenceTemplate.cs : 스크립터블오브젝트를 통해 오펜스 시 등장하는 적 오브젝트의 생성 패턴을 설정하는 코드입니다
+ OffenceWayCheck.cs : 오펜스에서 아군 오브젝트들의 생성 경로를 나타내는 코드입니다
+ OptionMenu.cs : 옵션창의 배경음 설정,효과음 설정,로비로 돌아가기와 같은 기능을 구현한 코드입니다
+ PlayerGold.cs : 인게임에서의 재화 기능을 담당하는 코드입니다
+ PlayerHP.cs : 인게임에서의 생명력 기능 및 게임 종료 시 페이드인/아웃 연출을 담당하는 코드입니다
+ Projectile.cs : 발사체의 발사 및 충돌 기능을 담당하는 코드입니다
+ RewardButton.cs : Admob의 보상형광고 기능을 유니티에서 사용 가능하도록 제작한 코드입니다
+ ScoreBoard.cs : 게임 종료 화면에서 점수,최고점수,획득한 재화량을 표시하는데 필요한 코드입니다
+ SettingVariable.cs : 게임을 시작할 때 최초로 게임 내에서 필요한 변수를 선언하고 PGS의 로그인과 데이터 불러오기 기능들을 실행시키는 코드입니다
+ SkillSpawner.cs : 영웅 유닛의 스킬을 사용할 시 작동하는 코드입니다
+ SliderPositionAutoSetter.cs : 오브젝트의 체력바 위치를 자동으로 위치시키는 코드입니다
+ SquadSetting.cs : 부대 편성 시 각 항목별로 보유한 영웅과 유닛을 표시하는 코드입니다
+ SquadShow.cs : 현재 편성된 영웅과 유닛을 보여주고 유닛과 영웅의 편성을 변경하는 기능을 담당하는 코드입니다
+ StartGame.cs : 타이틀 화면에서 게임 시작 시 페이드인/아웃 연출을 담당하는 코드입니다
+ SystemTextViewer.cs : 인게임에서 화면에 표시되는 시스템 메세지를 출력하는 코드입니다
+ TextTMPViewer.cs : 인게임에서 화면에 표시되는 텍스트 UI들을 출력하는 코드입니다
+ TileCheck.cs : 타일의 현재 체크상태를 확인하고 상황에 맞게 표시 연출을 출력하는 코드입니다
+ TMPAlpha.cs : 시스템 메세지의 텍스트 효과를 담당하는 코드입니다
+ TowerAttackRange.cs : 타워의 공격 범위를 시각적으로 보여주는 코드입니다
+ TowerDataViewer.cs : 타워 선택 시 표시되는 정보창 UI를 출력하고 타워 업그레이드 기능과 타워 판매 기능을 담당하는 코드입니다
+ TowerSpawner.cs : 타워 설치,타워 준비,설치 취소,설치시 체력바 생성과 같은 기능들을 담당하는 코드입니다
+ TowerTemplate.cs : 스크립터블오브젝트를 통해 영웅,유닛과 같은 타워 오브젝트의 능력치를 설정하는 코드입니다
+ TowerWeapon.cs : FSM 구조로 오브젝트의 탐지 및 공격 시스템을 제작한 코드입니다
+ UnitSquad.cs : 유닛과 영웅의 부대 편성 화면에서 해당 유닛과 영웅의 슬롯을 표시하고 터치시 선택하는 기능을 담당하는 코드입니다
+ WaveImage.cs : 인게임에서 현재 라운드 상태에 따라 디펜스/오펜스 이미지 UI를 표시하는 코드입니다
+ WaveSystem.cs : 라운드마다 디펜스와 오펜스 라운드를 실행시키는 코드입니다
---
## 3. 🪐 Dimension Squad

+ 2023년 6월 제작한 디멘션 스쿼드에서 사용된 코드들입니다
+ 장르 : 4인 CO-OP 로그라이트
+ 플레이 영상 : https://www.youtube.com/watch?v=yBrdbMJ5Jpw
+ 담당 업무 : 전반적인 게임의 모든 시스템 제작

### 💾 제작한 코드 설명
+ BulletSystem.cs : 발사체의 발사 및 충돌 기능을 담당하는 코드입니다
+ EffectSystem.cs : 이펙트 오브젝트의 발생 시 파괴를 담당하는 코드입니다
+ EnemySystem.cs : 적 오브젝트의 탐지,이동,공격,충돌,사망 처리를 담당하는 코드입니다
+ GameManager.cs : 인게임에서 사용되는 코드를 관리하고 적 캐릭터의 생성 및 플레이어의 능력치 시스템을 담당하는 싱글톤 패턴의 코드입니다
+ ItemData.cs : 스크립터블오브젝트를 통해 레벨업시 획득 가능한 아이템의 능력치를 설정하는 코드입니다
+ NetworkManager.cs : Photon PUN2를 통해 멀티플레이 환경을 구축하여 게임 접속,대기실 생성,대기실 입장,게임준비 확인,채팅 기능을 제작한 코드입니다
+ PartyMemberList.cs : 현재 대기실에 들어와 있는 플레이어들을 각 UI에 맞게 표시하는 코드입니다
+ PlayerStatus.cs : 스크립터블오브젝트를 통해 플레이어블 캐릭터의 능력치를 설정하는 코드입니다
+ PlayerSystem.cs : 플레이어블 캐릭터의 이동,공격,스킬,충돌,애니메이션 표시,능력치 증감을 담당하는 코드입니다
+ SkillData.cs : 스크립터블오브젝트를 통해 플레이어블 캐릭터가 보유한 스킬의 능력치를 설정하는 코드입니다
+ SkillSystem.cs : 플레이어블 캐릭터가 스킬을 사용할 시 스킬의 충돌 범위,이펙트 표시를 담당하는 코드입니다
---
## 4. 🚀 Astra

+ 2023년 12월 출시한 아스트라에서 사용된 코드들입니다
+ 장르 : TPS 액션 어드벤처
+ 다운로드(Google Play) : https://play.google.com/store/apps/details?id=gtc.gtcstudio.projectAstra&hl=ko
+ 플레이 영상 : https://www.youtube.com/watch?v=0CftThrA9TY
+ 담당 업무 : 필드의 오브젝트 생성 시스템,플레이어 캐릭터의 채집/전투와 같은 기능 제작,적 오브젝트의 AI 시스템 제작

### 💾 제작한 코드 설명
+ AreaData.cs : 오브젝트가 생성될 구역의 오브젝트 번호와 생성 숫자를 설정하는 코드입니다
+ EnemyData.cs : 스크립터블오브젝트를 통해 동물 오브젝트의 능력치를 설정하는 코드입니다
+ EnemySystem.cs : FSM 구조로 동물 오브젝트의 이동 및 탐지,공격과 같은 AI 기능을 담당하는 코드입니다
+ ExtensionMethods.cs : string 데이터를 특정 Enum으로 형변환하는 코드입니다
+ GameManager.cs : 인게임에서 사용되는 코드를 관리하고 플레이어의 능력치와 UI들의 출력,카메라 설정을 담당하는 싱글톤 패턴의 코드입니다
+ GameSetting.cs : 게임 내에서 사용되는 다양한 변수들을 선언하고 구글 스프레드시트로 만든 데이터테이블을 파싱하는 기능과 PGS의 로그인 기능,데이터 불러오기 기능이 실행되는 싱글톤 패턴의 코드입니다
+ Gathering.cs : 채집 오브젝트에게 채집 시 아이템을 드랍시키는 기능을 담당하는 코드입니다
+ GatheringSystem.cs : 채집 시 오브젝트가 채집이 가능한 오브젝트인지 체크하는 기능을 담당하는 코드입니다
+ InvenSlotView.cs : 필드에서 인벤토리창을 실행시켰을 때 인벤토리 UI를 출력시키는 코드입니다
+ Inventory.cs : 필드에서 아이템 획득 시 인벤토리에 추가하는 기능과,게임 종료 시 인벤토리의 아이템을 창고로 이동시키는 기능이 있고,획득 안내 문구 UI를 출력시키는 코드입니다
+ InvenUI.cs : 인벤토리 창의 슬롯 UI의 초기값을 설정하는 코드입니다
+ ItemData.cs : 스크립터블오브젝트를 통해 아이템의 필요 설정값들을 설정하는 코드입니다
+ ItemSlotView.cs : 인벤토리 창의 슬롯 UI를 출력하는 코드입니다
+ LinkedTable.cs : 구글 스프레드시트로 만든 데이터테이블들을 원하는 데이터에 파싱하는 기능을 담당하는 코드입니다
+ LoadingSystem.cs : 로딩 화면을 구현한 코드입니다
+ OptionSystem.cs : 옵션 화면 UI를 출력하고 해당 화면의 버튼들 기능을 담당하는 코드입니다
+ playerActing.cs : 플레이어의 전투,채집,애니메이션 기능을 담당하는 코드입니다
+ PoolManager.cs : 씬에 존재하는 오브젝트들을 관리하는 오브젝트 풀링 코드입니다
+ SaveCloud.cs : PGS(Play Games Service)에서 제공되는 기능을 통해 클라우드 저장을 사용하는 코드입니다
+ TouchEffect.cs : 로비에서 화면 터치 시 발생되는 터치 이펙트를 표시하는 코드입니다
+ UserData.cs : 스크립터블오브젝트를 통해 플레이어의 능력치나 재화,보유 아이템 데이터를 설정하는 코드입니다
+ WaterViewChange.cs : 물속에 들어갔을 때 화면을 변경하는 코드입니다
+ WeaponData.cs : 스크립터블오브젝트를 통해 무기 능력치를 설정하는 코드입니다
---
## 5. 🥚 Eggururu

+ 2024년 6월 출시한 에구루루에서 사용된 코드들입니다
+ 장르 : 캐주얼 러닝
+ 다운로드(Google Play) : https://play.google.com/store/apps/details?id=duj.teamDUJ.projectEggururu&hl=ko
+ 플레이 영상 : https://www.youtube.com/watch?v=zo9afTK6v6M
+ 담당 업무 : Firebase를 통한 로그인/유저 데이터 저장/랭킹/퀘스트 시스템,상점 시스템,로비 화면의 UI/UX

### 💾 제작한 코드 설명
+ AbilityShop.cs : 상점에서 특성 구매 시 랜덤한 특성을 획득하는 기능을 담당하는 코드입니다
+ CamLocation.cs : 로비 씬에서 UI 화면 전환 시 카메라 위치를 변경하는 코드입니다
+ Capture.cs : 현재 카메라가 보는 화면을 png 파일로 만드는 코드입니다
+ DatabaseManager.cs : Google Firebase의 실시간 데이터베이스를 불러와서 Dictionary형 변수에 집어넣는 기능을 담당하는 싱글톤 패턴의 코드입니다
+ Datatable.cs : 구글 스프레드시트로 만든 데이터테이블들을 원하는 데이터에 파싱하는 기능을 담당하는 싱글톤 패턴의 코드입니다
+ Eggskins.cs : 플레이어 캐릭터의 스킨을 적용하는 코드입니다
+ FirebaseAuthManager.cs : Google Firebase에서 제공되는 인증 기능을 유니티에서 사용 가능하도록 제작한 싱글톤 패턴의 코드입니다
+ FirestoreManager.cs : Google Firebase의 Firestore Database를 통해 유저 데이터를 저장/불러오기 기능을 사용하는 싱글톤 패턴의 코드입니다
+ GameMode.cs : 현재 게임의 게임모드를 설정하는 싱글톤 패턴의 코드입니다
+ LoadingSystem.cs : 로딩 화면을 구현한 코드입니다
+ Lobby.cs : 로비 씬에서 UI 출력 및 각 버튼들의 기능을 담당하는 코드입니다
+ LobbyRewardAds.cs : Admob의 보상형광고 기능을 유니티에서 사용 가능하도록 제작한 코드입니다
+ LoginSystem.cs : 게임 시작 시 타이틀 화면에서 로그인 기능 실행과 Admob 초기화,프레임 초기 설정을 담당하는 코드입니다
+ Option.cs : 옵션 화면 UI를 출력하고 해당 화면의 버튼들 기능을 담당하는 코드입니다
+ PostProcesing.cs : 시간 정지 아이템 발동 시 등장하는 연출 효과를 담당하는 코드입니다
+ QuestManager.cs : 퀘스트 시스템의 퀘스트 초기화,퀘스트 완료,퀘스트 진행 기능을 담당하는 싱글톤 패턴의 코드입니다
+ QuestShow.cs : 퀘스트 화면의 퀘스트 슬롯 UI를 출력하는 코드입니다
+ QuestSystem.cs : 퀘스트 화면의 진행 게이지 UI와 퀘스트 슬롯을 생성하는 코드입니다
+ RankingList.cs : 랭킹 화면의 순위 슬롯 UI를 출력하는 코드입니다
+ RankShow.cs : 클래식모드와 캐주얼모드,개인기록 랭킹 화면의 UI를 출력하는 코드입니다
+ SelectGameMode.cs : 게임 시작 진행 시 등장하는 팝업 화면의 UI를 출력하는 코드입니다
+ Shop.cs : 상점 화면의 전체적인 UI/UX 기능을 담당하는 코드입니다
+ ShopSlotList.cs : 상점 화면에서 스킨 상점 UI/UX를 담당하는 코드입니다
+ SkinShop.cs : 스킨 상점의 스킨 슬롯 UI/UX를 담당하는 코드입니다
+ SoundManager.cs : 배경음,효과음 기능을 담당하는 코드입니다
+ TitleCamera.cs : 타이틀 화면의 카메라 기능을 담당하는 코드입니다
+ UserData.cs : 플레이어의 게임 데이터를 담당하는 싱글톤 패턴의 코드입니다
