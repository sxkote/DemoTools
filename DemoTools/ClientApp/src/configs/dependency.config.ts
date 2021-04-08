import { Container } from "inversify";
import "reflect-metadata";
import SYMBOLS from "@/configs/symbols.ts";
import { IAuthenticationService, ILogger, IToken } from "@/interfaces/interfaces.ts";
import { AuthenticationService, SXLogger } from "@/services/authentication.service.ts";
import { Token } from "@/classes/token";
import { ITodoService } from "@/interfaces/main/todo.interface";
import { TodoService } from "@/services/main/todo.service";
import { IRegistrationService } from "../interfaces/main/registration.interface";
import { RegistrationService } from "../services/main/registration.service";

let container: Container = new Container();

container.bind<IToken>(SYMBOLS.IToken).to(Token);
container.bind<ILogger>(SYMBOLS.ILogger).to(SXLogger);
container.bind<IAuthenticationService>(SYMBOLS.IAuthenticationService).to(AuthenticationService);
container.bind<IRegistrationService>(SYMBOLS.IRegistrationService).to(RegistrationService);
container.bind<ITodoService>(SYMBOLS.ITodoService).to(TodoService);

export default container;