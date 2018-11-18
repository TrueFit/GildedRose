import { Degradation } from './degradation'

export interface Category {
    categoryID: number;
    name: string;
    maximumQuality: number;
    minimumQuality: number;
    Degradation: Degradation;
}