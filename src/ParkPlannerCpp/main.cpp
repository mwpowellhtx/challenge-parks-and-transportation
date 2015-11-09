#include "challenge.h"

#include <iostream>
#include <sstream>

#ifdef _DEVELOP

#define CRLF "\r\n"

std::vector<std::string> get_test_cases() {

    std::vector<std::string> result;

    // As expected C++ blows the snot out of C# hands down.

    // Expected output:
    // 176
    result.push_back("2 9" CRLF "95 91 88 77 67 53 52 46 46" CRLF "85 82 72 64 61 52 40 40 28"
        CRLF "1" CRLF "0" CRLF "2" CRLF "0 1");

    // Expected output:
    // 75
    // 50
    // IMPOSSIBLE
    result.push_back("5 3" CRLF "30 60 75" CRLF "30 15 30" CRLF "30 45 60" CRLF "60 45 15" CRLF "99 62 99"
        CRLF "3" CRLF "0" CRLF "3" CRLF "0 1 2" CRLF "55" CRLF "1" CRLF "3" CRLF "119" CRLF "1" CRLF "4");

    return result;
}

#endif

int main() {

    using challenges::park::challenge;

#ifdef _DEVELOP

    auto sep = false;

    for (auto const & tc : get_test_cases()) {

        if (sep)
            std::cout << "============================================" << std::endl;

        std::stringstream iss(tc);

        challenge c(&iss, &std::cout);

        sep = sep || true;
    }

#else

    challenge c(&std::cin, &std::cout);

#endif

    return 0;
}
