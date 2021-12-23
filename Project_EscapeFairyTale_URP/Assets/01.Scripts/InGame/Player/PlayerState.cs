public enum PlayerState
{
    NORMAL, // 일반 상태
    DEAD, // 죽었을때
    OPEN_INVENTORY, // 인벤토리를 열었을 때
    OPEN_BOOK, // 책 UI를 열었을 때
    PAUSED, // 일시 정지 상태
    WAKING_UP, // 일어나고 있을 때
    ENDING, // 엔딩 화면일때
    SLIPPING // 미끄러질때
}