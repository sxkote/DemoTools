import { Container } from "inversify";
import "reflect-metadata";
import SYMBOLS from "@/configs/symbols.ts";
import { AuthenticationService, SXLogger } from "@/services/authentication.service.ts";
import { Token } from "@/classes/token";
import { TodoService } from "@/services/main/todo.service";
import { RegistrationService } from "../services/main/registration.service";
let container = new Container();
container.bind(SYMBOLS.IToken).to(Token);
container.bind(SYMBOLS.ILogger).to(SXLogger);
container.bind(SYMBOLS.IAuthenticationService).to(AuthenticationService);
container.bind(SYMBOLS.IRegistrationService).to(RegistrationService);
container.bind(SYMBOLS.ITodoService).to(TodoService);
export default container;
//# sourceMappingURL=dependency.config.js.map