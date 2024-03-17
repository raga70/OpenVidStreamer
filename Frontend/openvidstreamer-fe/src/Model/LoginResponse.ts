export type AccountData = {
    accId: string;
    email: string;
    balance: number;
    subscriptionValidUntil: string;
};

export type LoginResponse = {
    item1: AccountData;
    item2: string;
};
