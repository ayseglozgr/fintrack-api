# Copilot Instructions

## Project Guidelines
- User prefers no assumptions on architecture decisions and wants clarifying questions in ambiguous situations before implementation.
- Architectural flow must follow: controller -> factory (if needed) -> service -> repository. Reject service -> factory or service -> same-level service patterns.
- Service classes should not call other same-level service classes directly; cross-service orchestration must use a factory abstraction (e.g., IUserHouseHoldFactory/UserHouseHoldFactory).
- Service or factory methods should return `ServiceResponse`; only helper methods may return non-`ServiceResponse` types.
- FinancialAccount must remain user-specific and independent of Household; do not add HouseholdId or household-bound constraints to the financialAccount table.
- Service Isolation: Services must only depend on their own primary repository. Direct cross-repository dependencies or DbContext injections in services are prohibited (e.g., UserService must only depend on IUserRepository). Follow commands/rules from the copilot-instr file consistently in subsequent changes.