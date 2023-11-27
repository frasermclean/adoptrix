import { Species } from "./api-responses";

export interface Animal {
  id: string;
  name: string;
  description?: string;
  species: Species;
  dateOfBirth: Date;
}
