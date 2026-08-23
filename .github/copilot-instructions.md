# Copilot Instructions

## Project Guidelines
- User prefers no assumptions on architecture decisions and wants clarifying questions in ambiguous situations before implementation.
- Architectural flow must follow: controller -> factory (if needed) -> service -> repository. Reject service -> factory or service -> same-level service patterns.
- Service classes should not call other same-level service classes directly; cross-service orchestration must use a factory abstraction (e.g., IUserHouseHoldFactory/UserHouseHoldFactory).
- Service or factory methods should return `ServiceResponse`; only helper methods may return non-`ServiceResponse` types.