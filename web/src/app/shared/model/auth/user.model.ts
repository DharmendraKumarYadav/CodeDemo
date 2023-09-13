export class User {
  constructor(id?: string, userName?: string, name?: string, email?: string,  phoneNumber?: string, role?: string) {
    this.id = id;
    this.userName = userName;
    this.fullName = name;
    this.email = email;
    this.phoneNumber = phoneNumber;
    this.role = role;
  }
  public id: string;
  public userName: string;
  public fullName: string;
  public email: string;
  public phoneNumber: string;
  public isEnabled: boolean;
  public isLockedOut: boolean;
  public role: string;
}
