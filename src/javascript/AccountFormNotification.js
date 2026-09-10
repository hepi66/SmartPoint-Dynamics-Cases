function showCompanyNumberNotification(executionContext) {
    var formContext = executionContext.getFormContext();
    var nameAttribute = formContext.getAttribute("name");
    var numberAttribute = formContext.getAttribute("cr0c9_firmennummer");
    var accountName = nameAttribute ? nameAttribute.getValue() : null;
    var companyNumber = numberAttribute ? numberAttribute.getValue() : null;
    var notificationId = "companyNumberNotification";

    if (accountName == null || String(accountName).trim() === "" ||
        companyNumber == null || String(companyNumber).trim() === "") {
        formContext.ui.clearFormNotification(notificationId);
        return;
    }

    formContext.ui.setFormNotification(
        "Die aktuelle Firmennummer der Firma " + accountName + " lautet " + companyNumber + ".",
        "INFO",
        notificationId
    );
}
