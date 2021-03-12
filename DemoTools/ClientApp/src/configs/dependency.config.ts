import { Container } from "inversify";
import "reflect-metadata";
import SYMBOLS from "@/configs/symbols.ts";
import { IAuthenticationService, ILogger, IToken } from "@/interfaces/interfaces.ts";
import { AuthenticationService, SXLogger3 } from "@/services/authentication.service.ts";
import { Token } from "../classes/token";

let container: Container = new Container();

container.bind<IToken>(SYMBOLS.IToken).to(Token);
container.bind<ILogger>(SYMBOLS.ILogger).to(SXLogger3);
container.bind<IAuthenticationService>(SYMBOLS.IAuthenticationService).to(AuthenticationService);

export default container;