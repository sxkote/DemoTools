import { Container } from "inversify";
import "reflect-metadata";
import SYMBOLS from "@/configs/symbols.ts";
import { AuthenticationService, SXLogger3 } from "@/services/authentication.service.ts";
import { Token } from "../classes/token";
let container = new Container();
container.bind(SYMBOLS.IToken).to(Token);
container.bind(SYMBOLS.ILogger).to(SXLogger3);
container.bind(SYMBOLS.IAuthenticationService).to(AuthenticationService);
export default container;
//# sourceMappingURL=dependency.config.js.map