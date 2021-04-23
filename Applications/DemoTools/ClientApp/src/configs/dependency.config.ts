import { Container } from "inversify";
import "reflect-metadata";
import SYMBOLS from "@/configs/symbols.ts";
import { IAuthenticationService, ICommonService, ILogger, IToken } from "@/interfaces/interfaces.ts";
import { AuthenticationService, SXLogger } from "@/services/authentication.service.ts";
import { Token } from "@/classes/token";
import { ITodoService } from "@/interfaces/main/todo.interface";
import { TodoService } from "@/services/main/todo.service";
import { IProfileService } from "../interfaces/main/profile.interface";
import { ProfileService } from "../services/main/profile.service";
import { CommonService } from "../services/common.service";

let container: Container = new Container();

container.bind<IToken>(SYMBOLS.IToken).to(Token);
container.bind<ILogger>(SYMBOLS.ILogger).to(SXLogger);
container.bind<ICommonService>(SYMBOLS.ICommonService).to(CommonService);
container.bind<IAuthenticationService>(SYMBOLS.IAuthenticationService).to(AuthenticationService);
container.bind<IProfileService>(SYMBOLS.IProfileService).to(ProfileService);
container.bind<ITodoService>(SYMBOLS.ITodoService).to(TodoService);

export default container;