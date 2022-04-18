# BackendApp
BackendApp


add-migration {Name} -Context ApplicationDbContext -O Persistence/Context/Migrations



Expression<Func<CustomerSurvey, bool>> expr1 = request.CustomerId.HasValue ? x => x.CustomerId == request.CustomerId.Value : null;
Expression<Func<CustomerSurvey, bool>> expr2 = request.SurveyOrderId.HasValue ? x => x.SurveyOrderId == request.SurveyOrderId.Value : null;

var exprCombo = expr1.And(expr2);

