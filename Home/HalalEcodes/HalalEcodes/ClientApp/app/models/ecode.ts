export class Ecode {
    id: number;
    code: string;
    category: string;
    ingredients: string;
    status: EcodeStatus;
    statusName: string;
    statusDesc: string;
    euApprouved: boolean;
    usApprouved: boolean;
    mainIngredient: string;
    containsAlcohol: boolean;
    isToxic: boolean;
}

export enum EcodeStatus {
    Halal = 0,
    Haram = 1,
    Mashbouh = 2,
    Unknown = 3
}