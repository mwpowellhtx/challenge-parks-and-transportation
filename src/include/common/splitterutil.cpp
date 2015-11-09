#include "splitterutil.h"
#include "strutil.h"
#include "parsers.h"

namespace challenges {

    std::vector<int> parse_ints(std::string const & s) {

        split splitter;

        std::vector<int> result;

        for (auto const x : splitter(s))
            result.push_back(to_int(x));

        return result;
    }
}
