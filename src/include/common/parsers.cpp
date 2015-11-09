#include "parsers.h"

#include <cstdlib>

namespace challenges {

    int to_int(std::string const & s) {
        //there are better ways to parse but this will do the trick for example purposes
        return atoi(s.c_str());
    }

    std::vector<int> to_ints(std::vector<std::string> const & values) {

        std::vector<int> result;

        for (auto const x : values)
            result.push_back(to_int(x));

        return result;
    }
}
