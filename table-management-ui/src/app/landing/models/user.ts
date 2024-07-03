export interface User {
    userId: number;
    firstName: string;
    lastName: string;
    email: string;
    password: string;
    token: string;
    active: boolean;
    manager: boolean;
}
