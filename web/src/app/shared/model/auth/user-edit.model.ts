import { User } from './user.model';

export class UserEdit extends User {
    constructor(currentPassword?: string, newPassword?: string, confirmPassword?: string) {
        super();

        this.currentPassword = currentPassword;
        this.newPassword = newPassword;
        this.confirmPassword = confirmPassword;
    }

    public currentPassword: string;
    public newPassword: string;
    public confirmPassword: string;
}
export class UserRegister extends User {
    constructor(password?: string) {
        super();
        this.password = password;
    }

    public password: string;
}