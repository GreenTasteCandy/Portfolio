# Portfolio
각 팀 프로젝트에서 참여하여 제작한 코드들을 모아놓은 Git입니다

---
## 1.EggururuClassic

+ 2022년 6월 출시된 에구루루(2022)에서 사용된 코드들입니다
+ 다운로드(Play Games) : 
+ 담당 업무 : 전반적인 게임의 모든 시스템 제작

### 제작한 코드 설명
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
## 2.Folast

+2022년 12월 출시된 포라스트에서 사용된 코드들입니다
+ 다운로드(Play Games) : 
+ 담당 업무 : PGS를 통한 저장 및 업적과 랭킹 시스템 구현,디펜스 시스템과 오펜스 시스템,아이템 선택 기능,오브젝트의 이동 및 전투 시스템,타워 생성 시스템

### 제작한 코드 설명
+ BuildTowerImage.cs : 인게임에서 유닛이나 영웅 버튼의 ui 표시 및 기능을 담당하는 코드입니다
+ CloudSave.cs : PGS(Play Games Service)에서 제공되는 기능을 통해 클라우드 저장을 사용하는 코드입니다
+ DefenceTemplate.cs : 스크립터블오브젝트를 통해 디펜스 시 등장하는 적 오브젝트의 생성 패턴을 설정하는 코드입니다
+ Enemy.cs : 오브젝트의 이동 기능을 담당하는 코드입니다
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
+ 
