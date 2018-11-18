export interface Degradation {
    DegradationID: number;
    threshold: number;
    interval: number;
    rate: number;
    hasNoValuePastExpiration: boolean;
}