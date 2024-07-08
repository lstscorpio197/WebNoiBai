const UserHelper = {
    USER_LEVEL: {
        ADMINISTRATOR: 0,
        LANHDAO: 1,
        TRUONGPHONG: 2,
        PHO_TRUONGPHONG: 3,
        DOITRUONG: 4,
        PHO_DOITRUONG: 5,
        NHANVIEN: 6
    },
    GetUserLevelDescription: (level) => {
        let levelDes = '';
        switch (level) {
            case UserHelper.USER_LEVEL.ADMINISTRATOR:
                levelDes = 'Quản trị hệ thống';
                break;
            case UserHelper.USER_LEVEL.LANHDAO:
                levelDes = 'Lãnh đạo';
                break;
            case UserHelper.USER_LEVEL.TRUONGPHONG:
                levelDes = 'Trưởng phòng';
                break;
            case UserHelper.USER_LEVEL.PHO_TRUONGPHONG:
                levelDes = 'Phó phòng';
                break;
            case UserHelper.USER_LEVEL.DOITRUONG:
                levelDes = 'Trưởng nhóm';
                break;
            case UserHelper.USER_LEVEL.PHO_DOITRUONG:
                levelDes = 'Phó nhóm';
                break;
            case UserHelper.USER_LEVEL.NHANVIEN:
                levelDes = 'Nhân viên';
                break;
            default:
                levelDes = '...';
                break;
        }
        return levelDes;
    }
}