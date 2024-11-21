export interface EditingOperationTypeDto {
    name?: string;
    estimatedDuration?: number;
    staffBySpecializations?: Record<string, number>;
  }